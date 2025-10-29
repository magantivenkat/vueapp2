using GoRegister.ApplicationCore.Domain.Registration.Services;
using System;
using System.Collections.Generic;
using AutoMapper;
using EFCore.BulkExtensions;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Framework.Domain;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Domain.BulkUpload.Models;
using Serilog;
using GoRegister.ApplicationCore.Domain.Delegates;

namespace GoRegister.ApplicationCore.Domain.BulkUpload.Services
{
    public interface IBulkUploadService
    {
        Task<Result<ResponseContainer<BulkUploadMappingModel, BulkUploadMappingViewModel>>> BuildBulkUploadMapping(Stream stream);
        Task<BulkUploadValidationResult> ValidateBulkUpload(BulkUploadMappingModel mappingModel, Stream stream);
        Task<BulkUploadExecutionResult> ExecuteBulkUpload(BulkUploadMappingModel mappingModel, Stream stream);
    }

    public class BulkUploadService : IBulkUploadService
    {
        private readonly IProjectSettingsAccessor _projectSettingsAccessor;
        private readonly IRepository _repository;
        private readonly IFormService _formService;
        private readonly IFieldDriverAccessor _fieldDriverAccessor;
        private readonly IAttendeeIdentifierService _attendeeIdentifierService;

        public BulkUploadService(IProjectSettingsAccessor projectSettingsAccessor, IRepository repository, IFormService formService, IFieldDriverAccessor fieldDriverAccessor, IAttendeeIdentifierService attendeeIdentifierService)
        {
            _projectSettingsAccessor = projectSettingsAccessor;
            _repository = repository;
            _formService = formService;
            _fieldDriverAccessor = fieldDriverAccessor;
            _attendeeIdentifierService = attendeeIdentifierService;
        }

        public async Task<Result<ResponseContainer<BulkUploadMappingModel, BulkUploadMappingViewModel>>> BuildBulkUploadMapping(Stream stream)
        {
            var model = new BulkUploadMappingModel();
            var viewModel = new BulkUploadMappingViewModel();

            var builderModel = await _formService.GetRegistrationForm();
            var fields = builderModel.Fields.Where(f=>!_fieldDriverAccessor.GetFormDriver(f.FieldTypeId).IsForPresentation);
            
            var columnMapping = new List<Tuple<int, Field>>();
            var workSheetResult = OpenWorksheet(stream);
            if (workSheetResult.Failed) return Result.Fail<ResponseContainer<BulkUploadMappingModel, BulkUploadMappingViewModel>>(workSheetResult.Error);

            var worksheet = workSheetResult.Value;
            var emailField = fields.FirstOrDefault(f => f.FieldTypeId == FieldTypeEnum.Email);
            if (emailField == null) return Result.Invalid<ResponseContainer<BulkUploadMappingModel, BulkUploadMappingViewModel>>("An email field must be configured on the registration form before you can bulk upload delegates");

            model.EmailFieldId = emailField.Id;

            if (worksheet.Dimension == null || worksheet.Dimension.End.Row == 1)
                return Result.Invalid<ResponseContainer<BulkUploadMappingModel, BulkUploadMappingViewModel>>("Upload contains no delegate data");

            for (int i = 1; i <= worksheet.Dimension.End.Column; i++)
            {
                var headerCell = worksheet.Cells[1, i, 1, i];
                var headerCellText = headerCell.Text?.Trim();

                // check if it is a system field
                if (String.Equals(headerCellText, "registration type", StringComparison.OrdinalIgnoreCase) ||
                    String.Equals(headerCellText, "registrationtype", StringComparison.OrdinalIgnoreCase))
                {
                    model.RegistrationTypeStatus = BulkUploadRegistrationTypeStatus.UseFromUpload;
                    model.RegistrationTypeColumnIndex = i;
                    continue;
                }

                int? fieldId = null;
                if (string.IsNullOrWhiteSpace(headerCellText))
                {
                    // use the header cell address if content is blank
                    headerCellText = headerCell.Address;
                }
                else
                {
                    // match on datatag then on name
                    var field = fields.FirstOrDefault(f => string.Equals(f.DataTag, headerCellText, StringComparison.OrdinalIgnoreCase))
                        ?? fields.FirstOrDefault(f => string.Equals(f.Name, headerCellText, StringComparison.OrdinalIgnoreCase));
                    fieldId = field?.Id;
                }

                model.HeaderMappings.Add(new HeaderMapping
                {
                    ColumnIndex = i,
                    ColumnName = headerCellText,
                    FieldId = fieldId,
                });
            }

            if (!model.HeaderMappings.Any())
            {
                return Result.Invalid<ResponseContainer<BulkUploadMappingModel, BulkUploadMappingViewModel>>("No headers found");
            }

            // get the reg types
            var regTypes = await _repository.DbContext.RegistrationTypes.ToListAsync();
            // if there are more than one reg type and we don't reg types coming in from the upload
            if (model.RegistrationTypeStatus != BulkUploadRegistrationTypeStatus.UseFromUpload && regTypes.Count > 1)
            {
                model.RegistrationTypeStatus = BulkUploadRegistrationTypeStatus.PleaseSelect;
                viewModel.RegistrationTypes = regTypes.Select(e => new TextValueModel(e.Id, e.Name));
            }

            viewModel.Fields = fields.Select(e => new BulkUploadField
            {
                Id = e.Id,
                DataTag = e.DataTag,
                Name = e.Name
            });

            return ResponseContainer.Ok(model, viewModel);
        }

        public async Task<BulkUploadValidationResult> ValidateBulkUpload(BulkUploadMappingModel mappingModel, Stream stream)
        {
            var workSheet = OpenWorksheet(stream).Value;
            var builderModel = await _formService.GetRegistrationForm();
            var bulkuploadWorksheet = await BulkUploadWorksheet.Create(_repository, (await _projectSettingsAccessor.GetAsync()), builderModel, mappingModel, workSheet, _fieldDriverAccessor, _formService, _attendeeIdentifierService);
            return await bulkuploadWorksheet.ValidateBulkUploadMapping();
        }

        public async Task<BulkUploadExecutionResult> ExecuteBulkUpload(BulkUploadMappingModel mappingModel, Stream stream)
        {
            var workSheet = OpenWorksheet(stream).Value;
            var builderModel = await _formService.GetRegistrationForm();
            var projectSettings = await _projectSettingsAccessor.GetAsync();
            var bulkuploadWorksheet = await BulkUploadWorksheet.Create(_repository, projectSettings, builderModel, mappingModel, workSheet, _fieldDriverAccessor, _formService, _attendeeIdentifierService);
            var responses = await bulkuploadWorksheet.BulkUpload();
            if (!bulkuploadWorksheet.ValidationResult.IsValid)
            {
                return new BulkUploadExecutionResult(bulkuploadWorksheet.ValidationResult);
            }

            var config = new BulkConfig
            {
                SetOutputIdentity = true,
                PreserveInsertOrder = true
            };

            using (var transaction = await _repository.BeginTransactionAsync())
            {
                var projectId = projectSettings.Id;
                var users = responses.Select(e => e.DelegateUser.ApplicationUser).ToList();
                var delegateUsers = responses.Select(e => e.DelegateUser).ToList();

                await _repository.BulkInsertAsync(users, config);

                foreach (var du in delegateUsers)
                {
                    du.Id = du.ApplicationUser.Id;
                    du.ParentDelegateUserId = null;
                    du.ProjectId = projectId;
                }

                await _repository.BulkInsertAsync(delegateUsers, config);

                foreach (var response in responses)
                {
                    response.UserId = response.DelegateUser.ApplicationUser.Id;
                    response.ProjectId = projectId;
                }

                await _repository.BulkInsertAsync(responses, config);

                var userFieldResponses = new List<UserFieldResponse>();
                foreach (var response in responses)
                {
                    foreach (var fieldResponse in response.UserFieldResponses)
                    {
                        fieldResponse.UserFormResponseId = response.Id;
                        fieldResponse.ProjectId = projectId;
                        userFieldResponses.Add(fieldResponse);
                    }
                }

                await _repository.BulkInsertAsync(userFieldResponses);

                // do we need to record this?
                //_repository.DbContext.BulkUploads.Add(new Data.Models.BulkUpload
                //{
                //    Id = Guid.NewGuid(),
                //    Status = BulkUploadStatusEnum.Uploaded,
                //    UserId = 1,
                //    DateCreatedUtc = SystemTime.UtcNow,
                //    DateUploadedUtc = SystemTime.UtcNow,
                //    FilePath = mappingModel.UploadId
                //});

                await transaction.CommitAsync();
            }

            return new BulkUploadExecutionResult(bulkuploadWorksheet.ValidationResult);
        }

        private Result<ExcelWorksheet> OpenWorksheet(Stream stream)
        {
            try
            {
                var excel = new ExcelPackage(stream);

                if (excel.Workbook.Worksheets.Any())
                    return Result.Ok(excel.Workbook.Worksheets.First());

                return Result.Invalid<ExcelWorksheet>("There must be at least one worksheet in the upload");
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Could not open sheet");
                return Result.Invalid<ExcelWorksheet>("There was an issue opening the file, please check it is a valid file");
            }
        }
    }
}

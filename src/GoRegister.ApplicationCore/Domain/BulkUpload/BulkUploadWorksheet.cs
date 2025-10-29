/*  MRF Changes : Create MRF Client Form details in Json format 
    Modified Date : 06th October 2022
    Modified By : Mandar.Khade@amexgbt.com
    Team member : Harish.Rane@amexgbt.com
    JIRA Ticket No : GoRegister/GOR-238  

 MRF Changes : Remove MRF Client Response details in Json format code
    Modified Date : 31st October 2022
    Modified By : Mandar.Khade@amexgbt.com
    Team member : Harish.Rane@amexgbt.com
    JIRA Ticket No : GoRegister/GOR-228   */


using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.BulkUpload.Models;
using GoRegister.ApplicationCore.Domain.BulkUpload.Specs;
using GoRegister.ApplicationCore.Domain.Delegates;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.BulkUpload
{
    public class BulkUploadWorksheet
    {
        private readonly IRepository _repository;
        private readonly Project _projectSettings;
        private readonly FormBuilderModel _builderModel;
        private readonly BulkUploadMappingModel _mappingModel;
        private readonly ExcelWorksheet _worksheet;
        private readonly IFieldDriverAccessor _fieldDriverAccessor;
        private readonly IFormService _formService;
        private readonly IAttendeeIdentifierService _attendeeIdentifierService;

        private List<RegistrationType> _registrationTypes;
        private Dictionary<string, RegistrationType> _registrationTypesDictionary;
        private IEnumerable<HeaderMapping> _headerMappingsToUse;

        public static async Task<BulkUploadWorksheet> Create(IRepository repository, Project projectSettings, FormBuilderModel builderModel, BulkUploadMappingModel mappingModel, ExcelWorksheet worksheet, IFieldDriverAccessor fieldDriverAccessor, IFormService formService, IAttendeeIdentifierService attendeeIdentifierService)
        {
            var upload = new BulkUploadWorksheet(repository, projectSettings, builderModel, mappingModel, worksheet, fieldDriverAccessor, formService, attendeeIdentifierService);
            await upload.Initialize();
            return upload;
        }

        private BulkUploadWorksheet(IRepository repository, Project projectSettings, FormBuilderModel builderModel, BulkUploadMappingModel mappingModel, ExcelWorksheet worksheet, IFieldDriverAccessor fieldDriverAccessor, IFormService formService, IAttendeeIdentifierService attendeeIdentifierService)
        {
            _repository = repository;
            _projectSettings = projectSettings;
            _builderModel = builderModel;
            _mappingModel = mappingModel;
            _worksheet = worksheet;
            _fieldDriverAccessor = fieldDriverAccessor;

            ValidationResult = new BulkUploadValidationResult();
            _headerMappingsToUse = _mappingModel.HeaderMappings.Where(hm => hm.FieldId != null);
            _formService = formService;
            _attendeeIdentifierService = attendeeIdentifierService;
        }

        public BulkUploadValidationResult ValidationResult { get; }

        public async Task Initialize()
        {
            _registrationTypes = await _repository.ToListAsync<RegistrationType>();
            _registrationTypesDictionary = _registrationTypes.ToDictionary(rt => rt.Name.ToUpperInvariant());
        }

        public async Task<BulkUploadValidationResult> ValidateBulkUploadMapping()
        {
            await ValidateWorksheet();
            for (var rowIndex = _worksheet.Dimension.Start.Row + 1; rowIndex <= _worksheet.Dimension.End.Row; rowIndex++)
            {
                await ProcessRow(rowIndex, true);
            }

            return ValidationResult;
        }

        public async Task<List<UserFormResponse>> BulkUpload()
        {
            var responses = new List<UserFormResponse>();
            var attendeeNumberRuns = 0;
            var attendeeNumbers = new HashSet<string>();
            var existingNumbers = new HashSet<string>(await _repository.DbContext.Delegates.Select(d => d.AttendeeNumber).ToListAsync());
            await ValidateWorksheet();
            for (var rowIndex = _worksheet.Dimension.Start.Row + 1; rowIndex <= _worksheet.Dimension.End.Row; rowIndex++)
            {
                var response = await ProcessRow(rowIndex, false);
                if (response == null) continue;

                response.DelegateUser.RegistrationDocument = _formService.SerializeForm(response, _builderModel);

                //response.DelegateUser.MRFClientResponse = _formService.SerializeFormRevised(response, _builderModel);

                while (true)
                {
                    if (attendeeNumberRuns > 500) throw new TimeoutException("Too many loops trying to generate attendee number");

                    var number = _attendeeIdentifierService.CreateAttendeeNumberForBulkUpload(null);
                    if (!existingNumbers.Contains(number) && attendeeNumbers.Add(number))
                    {
                        response.DelegateUser.AttendeeNumber = number;
                        break;
                    }
                    attendeeNumberRuns++;
                }

                responses.Add(response);
            }

            return responses;
        }

        private async Task ValidateWorksheet()
        {
            var fields = _builderModel.Fields;

            var emailFieldId = fields.First(f => f.FieldTypeId == FieldTypeEnum.Email).Id;
            var emailHeaderMapping = _mappingModel.HeaderMappings.FirstOrDefault(map => map.FieldId == emailFieldId);
            if (emailHeaderMapping == null)
            {
                ValidationResult.AddError("Email must be mapped");
                return;
            }

            // validate emails
            if (!_projectSettings.AllowDuplicateEmails)
            {
                var emails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var emailCells = _worksheet.Cells[_worksheet.Dimension.Start.Row + 1, emailHeaderMapping.ColumnIndex, _worksheet.Dimension.End.Row, emailHeaderMapping.ColumnIndex];
                var emailErrorCells = new List<string>();
                foreach (var emailCell in emailCells)
                {
                    if (!emails.Add(emailCell.Value.ToString().Trim()))
                    {
                        emailErrorCells.Add(emailCell.Address);
                    }
                }

                if (emailErrorCells.Any())
                {
                    ValidationResult.AddError("There are duplicate emails in the upload", emailErrorCells);
                }

                // we are limited to 2100 emails to check
                var emailChunks = new List<List<string>>();
                var emailsList = emails.ToList();
                for (int i = 0; i < emailsList.Count; i += 2000)
                {
                    emailChunks.Add(emailsList.GetRange(i, Math.Min(2000, emailsList.Count - i)));
                }

                var duplicateEmails = new List<string>();
                foreach (var emailChunk in emailChunks)
                {
                    duplicateEmails.AddRange(await _repository.SqlQueryAsync(new DuplicateEmailsSpecification(new { emails = emailChunk, projectId = _projectSettings.Id })));
                }

                if (duplicateEmails.Any())
                {
                    ValidationResult.AddError("There are emails in the upload that already exist in the system", duplicateEmails);
                }
            }

            if (_mappingModel.RegistrationTypeStatus == BulkUploadRegistrationTypeStatus.UseFromUpload)
            {
                if (!_mappingModel.RegistrationTypeColumnIndex.HasValue)
                {
                    ValidationResult.AddError("Could not find a Registration Type column, please double check your upload");
                }
            }
        }

        private async Task<UserFormResponse> ProcessRow(int rowIndex, bool justValidate)
        {
            var regType = ResolveRegistrationType(rowIndex);
            if (regType == null)
            {
                ValidationResult.AddError($"Row {rowIndex}: This row does not have a valid Registration Type");
                return null;
            }

            //var response = new UserFormResponse
            //{
            //    FormId = _builderModel.Form.Id,
            //    DelegateUser = DelegateUser.Create(regType.Id),
            //    ProjectId = _projectSettings.Id,
            //    CreatedUtc = SystemTime.UtcNow,
            //};

            //response.DelegateUser.ApplicationUser.ProjectId = response.ProjectId;

            var responseMRF = new UserFormResponse();

            responseMRF.FormId = _builderModel.Form.Id;
            responseMRF.DelegateUser = DelegateUser.Create(regType.Id);
            responseMRF.ProjectId = _projectSettings.Id;
            responseMRF.CreatedUtc = SystemTime.UtcNow;

            responseMRF.DelegateUser.ApplicationUser.ProjectId = responseMRF.ProjectId;


            //var context = new FormResponseContext();
            //context.Response = response;
            //context.BuilderModel = _builderModel;
            //context.SkipIsMandatoryCheck = true;
            //context.FormExecutionFrom = FormExecutionFrom.BulkUpload;

            var formResponseContext = new FormResponseContext();
            formResponseContext.UserFormResponseMRF = responseMRF;
            formResponseContext.BuilderModel = _builderModel;
            formResponseContext.SkipIsMandatoryCheck = true;
            formResponseContext.FormExecutionFrom = FormExecutionFrom.BulkUpload;

            var validationContext = new FormValidationContext();

            foreach (var col in _headerMappingsToUse)
            {
                var field = _builderModel.Fields.First(e => e.Id == col.FieldId.Value);
                var cell = _worksheet.Cells[rowIndex, col.ColumnIndex];
                var driver = _fieldDriverAccessor.GetFormDriver(field.FieldTypeId);

                validationContext.SetFieldContext(cell.Address);

                //if (justValidate)
                //    await driver.ValidateResponse(field, context, validationContext, cell);
                //else
                //    await driver.Process(field, context, validationContext, cell);

                if (justValidate)
                {
                    await driver.ValidateResponse(field, formResponseContext, validationContext, cell);
                }
                else
                {
                    await driver.Process(field, formResponseContext, validationContext, cell);
                }
            }

            if (!validationContext.IsValid)
            {
                ValidationResult.AddError($"Row {rowIndex}: we couldn't process this delegate", validationContext.Errors.Select(e => $"{e.Key}: {e.Value}").ToList());
            }

            return responseMRF;
        }

        private RegistrationType ResolveRegistrationType(int rowIndex)
        {
            switch (_mappingModel.RegistrationTypeStatus)
            {
                case BulkUploadRegistrationTypeStatus.PleaseSelect:
                    return _registrationTypes.First(e => e.Id == _mappingModel.RegistrationTypeId.Value);

                case BulkUploadRegistrationTypeStatus.UseFromUpload:
                    if (!_mappingModel.RegistrationTypeColumnIndex.HasValue) return null;
                    var cell = _worksheet.Cells[rowIndex, _mappingModel.RegistrationTypeColumnIndex.Value].Text?.Trim().ToUpperInvariant();
                    return _registrationTypesDictionary.ContainsKey(cell) ? _registrationTypesDictionary[cell] : null;

                case BulkUploadRegistrationTypeStatus.UseDefault:
                default:
                    return _registrationTypes.First();
            }
        }
    }
}

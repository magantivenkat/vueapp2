using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Translators;
using OfficeOpenXml;
using System;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using GoRegister.ApplicationCore.Data;
using System.Collections.Generic;
using GoRegister.ApplicationCore.Framework.Dapper;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.SingleSelect
{
    public class SingleSelectFormDriver : InputFormDriverBase<SingleSelectField, FieldOptionsEditModel, int, SingleSelectDisplayModel>
    {
        private readonly IFieldOptionCache _fieldOptionCache;
        private readonly ISlickConnection _slick;

        public SingleSelectFormDriver(IFieldOptionCache fieldOptionCache, ISlickConnection slick)
        {
            _fieldOptionCache = fieldOptionCache;
            _slick = slick;
        }

        public override FieldTypeEnum FieldType => FieldTypeEnum.RadioButton;

        public override IFormResponseTranslator<int> FormResponseTranslator => new IntFormResponseTranslator();

        public override DataStorageStrategyEnum StorageStrategy => DataStorageStrategyEnum.FieldOption;
        protected override string EditorTemplate => "OptionsEdit";
        protected override string EditorName => "Radio Button";


        //public override Task<SingleSelectDisplayModel> Display(SingleSelectDisplayModel model, SingleSelectField field, FieldDisplayContext context)
        //{
        //    var options = field.FieldOptions.Select(e => new FieldOptionDisplayModel(e.Id, e.Description));
        //    model.Options = options;
        //    if (field.SingleSelectType == SingleSelectField.SingleSelectTypeEnum.Radio)
        //    {
        //        model.Type = "radio-buttons";
        //    }
        //    else if (field.SingleSelectType == SingleSelectField.SingleSelectTypeEnum.Select)
        //    {
        //        model.Type = "select-list";
        //    }
        //    return Task.FromResult(model);
        //}

        public override Task<SingleSelectDisplayModel> Display(SingleSelectDisplayModel model, SingleSelectField field, FieldDisplayContext fieldDisplayContextMRF, int projectId = 0)
        {
            var options = field.FieldOptions.Select(e => new FieldOptionDisplayModel(e.Id, e.Description));
            model.Options = options;
            if (field.SingleSelectType == SingleSelectField.SingleSelectTypeEnum.Radio)
            {
                model.Type = "radio-buttons";
            }
            else if (field.SingleSelectType == SingleSelectField.SingleSelectTypeEnum.Select)
            {
                model.Type = "select-list";
            }
            return Task.FromResult(model);
        }

        public override ResponseResult<int> GetExcelResponse(SingleSelectField field, ExcelRange excelRange)
        {
            var text = excelRange.Text?.Trim();
            if (string.IsNullOrWhiteSpace(text)) return ResponseResult.Empty<int>();

            var val = field.FieldOptions.FirstOrDefault(e => e.Description.Equals(text, StringComparison.OrdinalIgnoreCase));
            if (val != null)
            {
                return ResponseResult.Ok(val.Id);
            }
            else
            {
                return ResponseResult.Fail<int>().WithMessage($"{text} is not a valid option for this field");
            }
        }

        //protected override void Process(SingleSelectField field, ResponseResult<int> response, FieldResponseContext context)
        //{
        //    if (response.HasValue)
        //        context.SetFieldOptionId(response.Value);
        //    else
        //        context.ClearFieldOptionId();
        //}

        protected override void Process(SingleSelectField field, ResponseResult<int> responseMRF, FieldResponseContext fieldResponseContextMRF)
        {
            if (responseMRF.HasValue)
                fieldResponseContextMRF.SetFieldOptionId(responseMRF.Value);
            else
                fieldResponseContextMRF.ClearFieldOptionId();
        }

        //public override Task ValidateAsync(SingleSelectField field, ResponseResult<int> response, FormValidationContext context, FieldResponseContext responseContext)
        //{
        //    if (response.HasValue)
        //    {
        //        var option = field.FieldOptions.FirstOrDefault(e => e.Id == response.Value);
        //        if (option == null || (option.IsDeleted && !responseContext.FormContext.IsAdmin))
        //        {
        //            context.AddError(field.Key, "Please select a valid option for {fieldName}");
        //        }
        //    }

        //    return Task.CompletedTask;
        //}

        public override Task ValidateAsync(SingleSelectField field, ResponseResult<int> responseMRF, FormValidationContext formValidationContextMRF, FieldResponseContext responseContext)
        {
            if (responseMRF.HasValue)
            {
                var option = field.FieldOptions.FirstOrDefault(e => e.Id == responseMRF.Value);
                if (option == null || (option.IsDeleted && !responseContext.FormContext.IsAdmin))
                {
                    formValidationContextMRF.AddError(field.Key, "Please select a valid option for {fieldName}");
                }
            }

            return Task.CompletedTask;
        }

        //protected override void Update(SingleSelectField field, FieldOptionsEditModel model, UpdateFormContext context)
        //{
        //    int optionOrder = 0;

        //    var idsToDelete = new List<int>();

        //    foreach (var opt in model.Options)
        //    {
        //        FieldOption dbOption;
        //        if (opt.TryGetDatabaseId(out int id))
        //        {
        //            dbOption = field.FieldOptions.FirstOrDefault(e => e.Id == id);
        //            if (dbOption == null) continue;

        //            if (opt.IsDeleted)
        //            {
        //                field.FieldOptions.Remove(dbOption);
        //                idsToDelete.Add(id);
        //                continue;
        //            }
        //        }
        //        else
        //        {
        //            if (opt.IsDeleted) continue;

        //            dbOption = new FieldOption();
        //            field.FieldOptions.Add(dbOption);
        //        }

        //        // replace with automapper?
        //        dbOption.Description = opt.Description;
        //        dbOption.IsDeleted = opt.IsDeleted;
        //        dbOption.SortOrder = optionOrder;
        //        dbOption.Capacity = opt.Capacity;
        //        dbOption.AdditionalInformation = opt.AdditionalInformation;
        //        dbOption.InternalInformation = opt.InternalInformation;

        //        // map back to field
        //        dbOption.Field = field;

        //        // set temp id for previewing
        //        dbOption.SetTempId(model.Id);

        //        //TODO: create a context to build a field option map
        //        context.AddOptionMap(opt.Id, dbOption);
        //        optionOrder++;
        //    }

        //    if(idsToDelete.Any())
        //    {
        //        context.PreSaveExecuteActions.Add(async () =>
        //        {
        //            using (var connection = _slick.Get())
        //            {
        //                await connection.ExecuteAsync(
        //                    @"delete from [UserFieldResponse] where [FieldOptionId] in @idsToDelete",
        //                    new { idsToDelete }
        //                );

        //                await connection.ExecuteAsync(
        //                    @"delete from [UserFieldResponseAudit] where [FieldOptionId] in @idsToDelete",
        //                    new { idsToDelete }
        //                );
        //            }
        //        });
        //    }
        //}

        protected override void Update(SingleSelectField field, FieldOptionsEditModel model, UpdateFormContext updateFormContextMRF)
        {
            int optionOrder = 0;

            var idsToDelete = new List<int>();

            foreach (var opt in model.Options)
            {
                FieldOption dbOption;
                if (opt.TryGetDatabaseId(out int id))
                {
                    dbOption = field.FieldOptions.FirstOrDefault(e => e.Id == id);
                    if (dbOption == null) continue;

                    if (opt.IsDeleted)
                    {
                        field.FieldOptions.Remove(dbOption);
                        idsToDelete.Add(id);
                        continue;
                    }
                }
                else
                {
                    if (opt.IsDeleted) continue;

                    dbOption = new FieldOption();
                    field.FieldOptions.Add(dbOption);
                }

                // replace with automapper?
                dbOption.Description = opt.Description;
                dbOption.IsDeleted = opt.IsDeleted;
                dbOption.SortOrder = optionOrder;
                dbOption.Capacity = opt.Capacity;
                dbOption.AdditionalInformation = opt.AdditionalInformation;
                dbOption.InternalInformation = opt.InternalInformation;

                // map back to field
                dbOption.Field = field;

                // set temp id for previewing
                dbOption.SetTempId(model.Id);

                //TODO: create a context to build a field option map
                updateFormContextMRF.AddOptionMap(opt.Id, dbOption);
                optionOrder++;
            }

            if (idsToDelete.Any())
            {
                updateFormContextMRF.PreSaveExecuteActions.Add(async () =>
                {
                    using (var connection = _slick.Get())
                    {
                        await connection.ExecuteAsync(
                            @"delete from [UserFieldResponse] where [FieldOptionId] in @idsToDelete",
                            new { idsToDelete }
                        );

                        await connection.ExecuteAsync(
                            @"delete from [UserFieldResponseAudit] where [FieldOptionId] in @idsToDelete",
                            new { idsToDelete }
                        );
                    }
                });
            }
        }

        //public override object GetCachedValue(int fieldId, DelegateUserCacheGetContext context)
        //{
        //    var optionIdVal = context.DelegateData.GetResponseValue(fieldId);
        //    if (optionIdVal != null && int.TryParse(optionIdVal.ToString(), out var optionId))
        //    {
        //        return _fieldOptionCache.Get(optionId)?.Description;
        //    };

        //    return null;
        //}

        public override object GetCachedValue(int fieldId, DelegateUserCacheGetContext delegateUserCacheGetContextMRF)
        {
            var optionIdVal = delegateUserCacheGetContextMRF.DelegateData.GetResponseValue(fieldId);
            if (optionIdVal != null && int.TryParse(optionIdVal.ToString(), out var optionId))
            {
                return _fieldOptionCache.Get(optionId)?.Description;
            };

            return null;
        }

        //public override object BuildUserResponses(UserResponseContext context)
        //{
        //    var foResult = context.Response.GetFieldOptionId(context.FieldId);
        //    if (foResult)
        //        return foResult.Value.ToString();

        //    return null;
        //}

        public override object BuildUserResponses(UserResponseContext userResponseContextMRF)
        {
            var foResult = userResponseContextMRF.UserFormResponseMRF.GetFieldOptionId(userResponseContextMRF.FieldId);
            if (foResult)
                return foResult.Value.ToString();

            return null;
        }
    }
}

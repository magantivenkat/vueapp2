using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Translators;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using GoRegister.ApplicationCore.Domain.Countries;
using GoRegister.ApplicationCore.Domain.Registration.Fields.SingleSelect;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.Country
{
    public class CountryFormDriver : InputFormDriverBase<CountryField, FieldEditorModel, string, SingleSelectDisplayModel>
    {
        private readonly ICountryCacheService _cache;

        public CountryFormDriver(ICountryCacheService cache)
        {
            _cache = cache;
        }

        public override FieldTypeEnum FieldType => FieldTypeEnum.Country;

        public override IFormResponseTranslator<string> FormResponseTranslator => new StringFormResponseTranslator();

        public override DataStorageStrategyEnum StorageStrategy => DataStorageStrategyEnum.Unknown;
        protected override string EditorTemplate => "";


        public override async Task<SingleSelectDisplayModel> Display(SingleSelectDisplayModel model, CountryField field, FieldDisplayContext context, int projectId = 0)
        {
            var options = (await _cache.GetCountries()).Select(e => new FieldOptionDisplayModel(e.ISO, e.Name)).OrderBy(e => e.Text);
            model.Options = options;
            model.Type = "select-list";
            return model;
        }

        public override async Task<ResponseResult<string>> GetExcelResponseAsync(CountryField field, ExcelRange excelRange)
        {
            var countries = _cache.GetCountries();
            var excelValue = excelRange.Text?.Trim();

            if (string.IsNullOrWhiteSpace(excelValue)) return ResponseResult.Empty<string>();

            CountryModel model = excelValue.Length == 2
                ? await _cache.GetCountry(excelValue)
                : (await _cache.GetCountries()).FirstOrDefault(e => e.Name.Equals(excelValue, StringComparison.OrdinalIgnoreCase));

            if (model != null)
            {
                return ResponseResult.Ok(model.ISO);
            }
            else
            {
                return ResponseResult.Fail<string>().WithMessage($"{excelValue} is not a valid value for this field");
            }
        }

        //protected override void Process(CountryField field, ResponseResult<string> response, FieldResponseContext context)
        //{
        //    //var formResponse = context.FormContext.Response;

        //    var formResponse = context.FormContext.Responses;

        //    if (response.HasValue)
        //    {
        //        var fieldResponse = formResponse.GetResponse(field.Id);
        //        if (fieldResponse == null)
        //        {
        //            fieldResponse = new UserFieldResponse(field.Id);
        //            fieldResponse.FieldId = field.Id;
        //            formResponse.UserFieldResponses.Add(fieldResponse);
        //        }
        //        else
        //        {
        //            if (fieldResponse.CountryId == response.Value) return;
        //        }

        //        fieldResponse.CountryId = response.Value;
        //        formResponse.AddAudit(fieldResponse);
        //    }
        //    else
        //        context.ClearResponse();
        //}

        protected override void Process(CountryField field, ResponseResult<string> responseMRF, FieldResponseContext fieldResponseContextMRF)
        {
            //var formResponse = context.FormContext.Response;

            var formResponse = fieldResponseContextMRF.FormContext.UserFormResponseMRF;

            if (responseMRF.HasValue)
            {
                var fieldResponse = formResponse.GetResponse(field.Id);
                if (fieldResponse == null)
                {
                    fieldResponse = new UserFieldResponse(field.Id);
                    fieldResponse.FieldId = field.Id;
                    formResponse.UserFieldResponses.Add(fieldResponse);
                }
                else
                {
                    if (fieldResponse.CountryId == responseMRF.Value) return;
                }

                fieldResponse.CountryId = responseMRF.Value;
                formResponse.AddAudit(fieldResponse);
            }
            else
                fieldResponseContextMRF.ClearResponse();
        }

        //public override async Task<object> GetCachedValueAsync(int fieldId, DelegateUserCacheGetContext context)
        //{
        //    var ob = context.DelegateData.GetResponseValue(fieldId);
        //    if (ob == null) return null;
        //    var val = (string)ob;

        //    return (await _cache.GetCountry(val))?.Name;
        //}

        public override async Task<object> GetCachedValueAsync(int fieldId, DelegateUserCacheGetContext delegateUserCacheGetContextMRF)
        {
            var ob = delegateUserCacheGetContextMRF.DelegateData.GetResponseValue(fieldId);
            if (ob == null) return null;
            var val = (string)ob;

            return (await _cache.GetCountry(val))?.Name;
        }

        //public override object BuildUserResponses(UserResponseContext context)
        //{
        //    var response = context.Response.GetResponse(context.FieldId);
        //    if (response != null)
        //    {
        //        return response.CountryId;
        //    }

        //    return null;
        //}

        public override object BuildUserResponses(UserResponseContext userResponseContextMRF)
        {
            var responseMRF = userResponseContextMRF.UserFormResponseMRF.GetResponse(userResponseContextMRF.FieldId);
            if (responseMRF != null)
            {
                return responseMRF.CountryId;
            }

            return null;
        }
    }
}

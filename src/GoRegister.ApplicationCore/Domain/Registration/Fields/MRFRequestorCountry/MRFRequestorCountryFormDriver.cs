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
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using GoRegister.ApplicationCore.Data.Models.Fields;
using System.Net.Http.Headers;
using GoRegister.ApplicationCore.Data;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.MRFRequestorCountry
{

    public class MRFRequestorCountryFormDriver : InputFormDriverBase<MRFRequestorCountryField, FieldEditorModel, string, SingleSelectDisplayModel>
    {
        private readonly ICountryCacheService _cache;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;

        public MRFRequestorCountryFormDriver(ICountryCacheService cache, IConfiguration configuration, ApplicationDbContext db)
        {
            _cache = cache;
            _configuration = configuration;
            _db = db;
        }

        public override FieldTypeEnum FieldType => FieldTypeEnum.MRFRequestorCountry;

        public override IFormResponseTranslator<string> FormResponseTranslator => new StringFormResponseTranslator();

        public override DataStorageStrategyEnum StorageStrategy => DataStorageStrategyEnum.Unknown;
        protected override string EditorTemplate => "";

        public override async Task<SingleSelectDisplayModel> Display(SingleSelectDisplayModel model, MRFRequestorCountryField field, FieldDisplayContext context, int projectId = 0)
        {
            var countries = await _cache.GetCountries();
            //var options = countries.Select(e => new FieldOptionDisplayModel(e.ISO, e.Name)).OrderBy(e => e.Text);
            var requestorCountries = await _db.MRFServiceCountryMapping.Where(e => e.ProjectId == projectId && e.IsActive==true).ToListAsync();
            var options = requestorCountries
                .Select(e => new FieldOptionDisplayModel(e.RequestCountryId, e.RequestCountry))
                .OrderBy(e => e.Text);
            model.Options = options.ToList();
            model.Type = "select-list";
            return model;
        }

        //protected override void Process(MRFServicingCountryField field, ResponseResult<string> response, FieldResponseContext context)
        //{
        //    var formResponse = context.FormContext.Response;
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
        //            if (fieldResponse.StringValue == response.Value) return;
        //        }

        //        fieldResponse.StringValue = response.Value;
        //        formResponse.AddAudit(fieldResponse);
        //    }
        //    else
        //        context.ClearResponse();
        //}

        protected override void Process(MRFRequestorCountryField field, ResponseResult<string> responseMRF, FieldResponseContext fieldResponseContextMRF)
        {
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
                    if (fieldResponse.StringValue == responseMRF.Value) return;
                }

                fieldResponse.StringValue = responseMRF.Value;
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

        //    return (await _cache.GetCountryMRF(val))?.CountryName;

        //}

        public override async Task<object> GetCachedValueAsync(int fieldId, DelegateUserCacheGetContext delegateUserCacheGetContextMRF)
        {
            var ob = delegateUserCacheGetContextMRF.DelegateData.GetResponseValue(fieldId);
            if (ob == null) return null;
            var val = (string)ob;

            return (await _cache.GetCountryMRF(val))?.CountryName;

        }


        //public override object BuildUserResponses(UserResponseContext context)
        //{
        //    var response = context.Response.GetResponse(context.FieldId);
        //    if (response != null)
        //    {
        //        return response.StringValue;
        //    }
        //    return null;
        //}

        public override object BuildUserResponses(UserResponseContext userResponseContextMRF)
        {
            var responseMRF = userResponseContextMRF.UserFormResponseMRF.GetResponse(userResponseContextMRF.FieldId);
            if (responseMRF != null)
            {
                return responseMRF.StringValue;
            }
            return null;
        }
    }
}

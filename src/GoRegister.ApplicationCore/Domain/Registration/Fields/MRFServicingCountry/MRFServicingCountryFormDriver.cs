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

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.MRFServicingCountry
{

    public class MRFServicingCountryFormDriver : InputFormDriverBase<MRFServicingCountryField, FieldEditorModel, string, SingleSelectDisplayModel>
    {
        private readonly ICountryCacheService _cache;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;

        public MRFServicingCountryFormDriver(ICountryCacheService cache, IConfiguration configuration, ApplicationDbContext db)
        {
            _cache = cache;
            _configuration = configuration;
            _db = db;
        }

        public override FieldTypeEnum FieldType => FieldTypeEnum.MRFServicingCountry;

        public override IFormResponseTranslator<string> FormResponseTranslator => new StringFormResponseTranslator();

        public override DataStorageStrategyEnum StorageStrategy => DataStorageStrategyEnum.Unknown;
        protected override string EditorTemplate => "";

        public override async Task<SingleSelectDisplayModel> Display(SingleSelectDisplayModel model, MRFServicingCountryField field, FieldDisplayContext context, int projectId = 0)
        {
            var mrfSubsidiariesDetailsAPI = this._configuration.GetSection("APIDetails")["MRFSubsidiariesDetailsAPI"];
            var accessToken = this._configuration.GetSection("APIDetails")["AccessToken"];
            var mrfMeetingTypeAPI = this._configuration.GetSection("APIDetails")["MRFMeetingTypeAPI"];

            var project = await _db.Projects.SingleOrDefaultAsync(p => p.Id == field.ProjectId);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(mrfSubsidiariesDetailsAPI + "/" + project.Prefix);
            client.DefaultRequestHeaders.Add("Authorization", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = client.GetAsync(client.BaseAddress))
            {
                if (response.Result.IsSuccessStatusCode)
                {
                    var fileJsonString = response.Result.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<MRFSingleSelectFieldOptions>>(fileJsonString.Result);
                    var options = data.Select(e => new FieldOptionDisplayModel(e.UUId, e.Name)).OrderBy(e => e.Text);
                                        
                    if(field.AllowTPNCountries==true)
                    {
                        var TPNClientList = await _db.TPNCountryClientEmails.Where(e => e.ClientUuid == project.Prefix).ToListAsync();
                        var allOptions = options.Concat(TPNClientList.Where(c=>c.IsDeleted==false).GroupBy(e => new { e.TPNCountry }).Select(group => new FieldOptionDisplayModel(group.First().TPNCountry, (group.First().TPNCountry))).Distinct());
                        model.Options = allOptions;
                    }
                    else
                    {
                        model.Options = options;
                    }                                  
                    
                }
            }

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

        protected override void Process(MRFServicingCountryField field, ResponseResult<string> responseMRF, FieldResponseContext fieldResponseContextMRF)
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

/*  MRF Changes : Create MRF Client Form details in Json format 
    Modified Date : 06th October 2022
    Modified By : Mandar.Khade@amexgbt.com
    Team member : Harish.Rane@amexgbt.com
    JIRA Ticket No : GoRegister/GOR-238  
    
    MRF Changes : Add new function "SerializeFormMRF" to add UUID field details into MRF Client Form details in Json format 
    Modified Date : 31st October 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-228 
 
 */

using AutoMapper;
using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Countries;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Framework.Dapper;
using GoRegister.ApplicationCore.Framework.Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Services
{
    public interface IFormService
    {
        Task<RegistrationSummaryModel> BuildFormSummary(DelegateDataTagAccessor delegateUser, FormType formType, bool isAdmin = false);
        Task<Result<UserFormResponseModel>> CreateUserResponseModel(int formId, int userId);
        Task<FormSchema> GenerateFormSchema(int regTypeId, IFormBuilderModel builderModel);
        Task<FormBuilderModel> GetCancellationForm();
        Task<FormBuilderModel> GetForm(int id);
        Task<FormBuilderModel> GetForm(FormType formType);
        Task<FormModel> GetFormDisplayModel(IFormBuilderModel builderModel, UserFormResponseModel userFormResponse);
        Task<FormBuilderModel> GetRegistrationForm();
        Task<Result<UserFormResponseModel>> GetUserResponseModel(int formId, int userId);
        Task<FormValidationContext> ProcessFormResponse(FormBuilderModel builderModel, UserFormResponseModel userFormResponse, FormModel form);
        string SerializeForm(UserFormResponse formResponse, FormBuilderModel formBuilder);
        string SerializeFormMRF(UserFormResponse formResponse, FormBuilderModel formBuilder, string UUID);
        List<MRFFormDetails> SerializeFormMRFEmail(UserFormResponse formResponse, FormBuilderModel formBuilder, string UUID, bool isTPNCountry);
    }

    public class FormService : IFormService
    {
        private readonly ApplicationDbContext _db;
        private readonly IEnumerable<IFormDriver> _formDrivers;
        private readonly IMapper _mapper;
        private readonly IProjectSettingsAccessor _projectSettingsAccessor;
        private readonly ISlickConnection _slickConnection;
        private readonly ICountryCacheService _cache;

        public FormService(ApplicationDbContext db, IEnumerable<IFormDriver> formDrivers, IMapper mapper, IProjectSettingsAccessor projectSettingsAccessor, ISlickConnection slickConnection, ICountryCacheService cache)
        {
            _db = db;
            _formDrivers = formDrivers;
            _mapper = mapper;
            _projectSettingsAccessor = projectSettingsAccessor;
            _slickConnection = slickConnection;
            _cache = cache;
        }

        public async Task<FormBuilderModel> GetForm(FormType formType)
        {
            var form = await _db.Forms
                .Where(e => e.FormTypeId == formType)
                .FirstOrDefaultAsync();

            return await GetForm(form);
        }

        public async Task<FormBuilderModel> GetRegistrationForm()
        {
            var form = await _db.Forms
                .Where(e => e.FormTypeId == FormType.Registration)
                .FirstOrDefaultAsync();

            return await GetForm(form);
        }

        public async Task<FormBuilderModel> GetCancellationForm()
        {
            var form = await _db.Forms
                .Where(e => e.FormTypeId == FormType.Cancel)
                .FirstOrDefaultAsync();

            return await GetForm(form);
        }

        public async Task<FormBuilderModel> GetForm(int id)
        {
            var form = await _db.Forms
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            return await GetForm(form);
        }

        private async Task<FormBuilderModel> GetForm(Form form)
        {
            var formModel = new FormBuilderModel();
            formModel.Form = form;
            formModel.Fields = await GetFieldsQuery()
                .Where(e => e.RegistrationPage.FormId == form.Id)
                .ToListAsync();



            // gathering pages from fields
            var pageIds = new HashSet<string>();
            var pages = new List<RegistrationPage>();
            foreach (var field in formModel.Fields)
            {

                field.FieldOptions = field.FieldOptions.OrderBy(e => e.SortOrder).ToList();

                // continue if page has already been added
                if (pageIds.Contains(field.GetPageRenderId())) continue;

                pages.Add(field.RegistrationPage);
                pageIds.Add(field.GetPageRenderId());
            }
            formModel.Pages = pages.OrderBy(e => e.SortOrder).ToList();

            formModel.Rules = (await _db.FieldOptionRules
                .Where(e => e.FieldOption.Field.RegistrationPage.FormId == form.Id)
                .ToListAsync())
                .Select(e => new FormRuleModel
                {
                    Id = e.Id,
                    FieldOptionId = e.FieldOptionId.ToString(),
                    FieldId = e.FieldOption.FieldId.ToString(),
                    NextFieldId = e.NextFieldId?.ToString() ?? "",
                    NextFieldOptionId = e.NextFieldOptionId?.ToString() ?? "",
                    IsHidden = e.FieldOption.Field.IsHidden
                }).ToList();

            return formModel;
        }

        public async Task<Result<UserFormResponseModel>> CreateUserResponseModel(int formId, int userId)
        {
            var user = await _db.Delegates
                    .Include(e => e.ApplicationUser)
                    .Include(e => e.RegistrationType)
                        .ThenInclude(e => e.RegistrationPath)
                    .Where(e => e.Id == userId)
                    .SingleOrDefaultAsync();

            var response = new UserFormResponse();
            response.FormId = formId;
            response.DelegateUser = user;
            var model = new UserFormResponseModel(response);
            return Result.Ok(model);
        }

        public async Task<Result<UserFormResponseModel>> GetUserResponseModel(int formId, int userId)
        {
            var response = await _db.UserFormResponses
                .Include(e => e.UserFieldResponses)
                .Include(e => e.DelegateUser)
                    .ThenInclude(e => e.ApplicationUser)
                .Include(e => e.DelegateUser)
                    .ThenInclude(e => e.RegistrationType)
                        .ThenInclude(e => e.RegistrationPath)
                .Where(e => e.UserId == userId && e.FormId == formId)
                .FirstOrDefaultAsync();

            if (response == null)
            {
                return await CreateUserResponseModel(formId, userId);
            }

            var model = new UserFormResponseModel(response);
            return Result.Ok(model);
        }

        public async Task<FormModel> GetFormDisplayModel(IFormBuilderModel builderModel, UserFormResponseModel userFormResponse)
        {
            var response = userFormResponse.Response;

            var schemas = new Dictionary<int, FormSchema>();
            foreach (var regTypeId in userFormResponse.GetRegistrationTypeIds())
            {
                schemas.Add(regTypeId, await GenerateFormSchema(regTypeId, builderModel));
            }

            var mainUser = await GenerateUserModel(response, schemas[response.DelegateUser.RegistrationTypeId]);

            //var languages = await GetLanguagesAsync();

            var model = new FormModel();
            model.User = mainUser;
            model.FormSchemas = schemas;
            //model.LookupData = new LookupDataModel { Languages = languages };

            return model;
        }

        private async Task<FormResponseModel> GenerateUserModel(UserFormResponse ufr, FormSchema schema)
        {
            var userData = new Dictionary<string, JToken>();
            foreach (var field in schema.Fields)
            {
                var driver = GetFormDriver(field.FieldTypeId);
                //TODO: add database id to field display model
                var responseContext = new UserResponseContext(ufr, int.Parse(field.Id));
                var response = await driver.BuildUserResponsesAsync(responseContext);
                if (response != null)
                {
                    userData.TryAdd(field.Key, JToken.FromObject(response, new JsonSerializer
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }));
                }
            }

            return new FormResponseModel
            {
                Id = ufr.DelegateUser?.Id,
                RegistrationTypeId = ufr.DelegateUser.RegistrationTypeId,
                Model = userData
            };
        }

        public async Task<RegistrationSummaryModel> BuildFormSummary(DelegateDataTagAccessor delegateUser, FormType formType, bool isAdmin = false)
        {
            var fields = await _db.Fields
                .Include(e => e.FieldOptions)
                .OrderBy(e => e.SortOrder)
                .Where(e => !e.IsDeleted)
                .Where(e => e.RegistrationPage.Form.FormTypeId == formType)
                .ToListAsync();

            var summary = new List<RegistrationFieldSummaryModel>();
            foreach (var f in fields)
            {
                if (!isAdmin && f.IsHidden) continue;

                var driver = GetFormDriver(f.FieldTypeId);
                if (driver.IsForPresentation) continue;

                var model = new RegistrationFieldSummaryModel
                {
                    DataTag = f.DataTag,
                    Name = f.Name
                };

                var value = await driver.GetSummaryValueAsync(f, new DelegateUserCacheGetContext { DelegateData = delegateUser });
                if (value != null)
                {
                    model.Value = value.ToString();
                }

                summary.Add(model);
            }

            return new RegistrationSummaryModel
            {
                Fields = summary,
                UserId = delegateUser.Id
            };
        }

        public async Task<FormSchema> GenerateFormSchema(int regTypeId, IFormBuilderModel builderModel)
        {
            var context = new FormDisplayContext(_mapper, null, builderModel.IsAdmin);
            var requiredPages = new HashSet<string>();

            var ruleExecutor = new FieldRuleExecutor(GetFormDrivers(), builderModel.Rules.Where(e => e.IsHidden));

            var results = new List<IFieldDisplayModel>();
            int projectId = builderModel.Form.ProjectId;
            foreach (var field in builderModel.Fields)
            {
                // only generate field schema if field has no reg types or has the specific one selected
                if (field.RegistrationTypeFields.Any())
                {
                    if (!field.RegistrationTypeFields.Select(e => e.RegistrationTypeId).Contains(regTypeId)) continue;
                }

                // only generate schema for internal fields if doing it in admin
                if (!builderModel.IsAdmin && field.IsHidden)
                {
                    //TODO: here we need to add support for internal fields and rules
                    continue;
                }

                //ruleExecutor.ShouldDisplayField(field.GetRenderId(), )

                requiredPages.Add(field.GetPageRenderId());

                var driver = GetFormDriver(field.FieldTypeId);
                var result = await driver.Render(field, context, projectId);
                if (result == null) continue;
                //TODO: move into form driver
                var rulesForField = builderModel.Rules.Where(e => e.NextFieldId == field.GetRenderId());
                if (rulesForField.Any())
                {
                    result.Model.Rules = rulesForField.GroupBy(e => e.FieldId).Select(e => new Framework.FieldRule { Id = e.Key, Values = e.Select(x => x.FieldOptionId) });
                }

                results.Add(result.Model);
            }

            var pages = new List<FormPageDisplayModel>();
            foreach (var page in builderModel.Pages)
            {
                var pageId = page.GetRenderId();
                // we can skip pages that have no fields
                if (!requiredPages.Contains(pageId)) continue;

                pages.Add(new FormPageDisplayModel
                {
                    Id = page.GetRenderId(),
                    Title = page.Title
                });
            }

            return new FormSchema
            {
                IsAdmin = builderModel.IsAdmin,
                Fields = results,
                Pages = pages,
                EnableReview = !builderModel.Form.IsReviewPageHidden,
                SubmitButton = builderModel.Form.SubmitButtonText
            };
        }

        //public async Task<FormValidationContext> ProcessFormResponse(FormBuilderModel builderModel, UserFormResponseModel userFormResponse, FormModel form)
        //{
        //    // accept form builder model, client and db form response
        //    var settings = await _projectSettingsAccessor.GetAsync();
        //    var fieldRuleExecutor = new FieldRuleExecutor(GetFormDrivers(), builderModel.Rules);

        //    //TODO: return validation
        //    async Task<FormValidationContext> Process(UserFormResponseModel dbModel, FormResponseModel response)
        //    {
        //        var context = new FormResponseContext();
        //        context.IsAdmin = builderModel.IsAdmin;
        //        context.SkipIsMandatoryCheck = builderModel.IsAdmin;
        //        context.Response = dbModel.Response;
        //        context.BuilderModel = builderModel;
        //        context.FieldRuleExecutor = fieldRuleExecutor;

        //        var validationContext = new FormValidationContext();

        //        foreach (var field in builderModel.Fields)
        //        {
        //            validationContext.SetFieldContext(field.GetRenderId());
        //            var driver = GetFormDriver(field.FieldTypeId);
        //            await driver.Process(field, context, validationContext, response.Model);
        //        }

        //        return validationContext;
        //    }

        //    //TODO: Check validation
        //    var validationContext = await Process(userFormResponse, form.User);

        //    if (!validationContext.IsValid)
        //    {

        //    }

        //    if (userFormResponse.Response.Id == 0 && userFormResponse.Response.FormId != 0)
        //    {
        //        _db.UserFormResponses.Add(userFormResponse.Response);
        //    }

        //    //TODO: add support for guest registration
        //    //foreach (var guest in form.User.Guests)
        //    //{
        //    //    DelegateUser guestUser;
        //    //    if (guest.Value.Id != null)
        //    //    {
        //    //        guestUser = delegateContainer.Guests.FirstOrDefault(e => e.Id == guest.Value.Id);
        //    //    }
        //    //    else
        //    //    {
        //    //        guestUser = DelegateUser.Create(guest.Value.RegistrationTypeId);
        //    //    }

        //    //    //TODO: check validation
        //    //    await Process(guestUser, guest.Value);
        //    //}

        //    return validationContext;
        //}

        public async Task<FormValidationContext> ProcessFormResponse(FormBuilderModel builderModel, UserFormResponseModel userFormResponse, FormModel form)
        {
            // accept form builder model, client and db form response
            var settings = await _projectSettingsAccessor.GetAsync();
            var fieldRuleExecutor = new FieldRuleExecutor(GetFormDrivers(), builderModel.Rules);

            //TODO: return validation
            async Task<FormValidationContext> Process(UserFormResponseModel dbModel, FormResponseModel response)
            {
                var context = new FormResponseContext();
                context.IsAdmin = builderModel.IsAdmin;
                context.SkipIsMandatoryCheck = builderModel.IsAdmin;
                context.UserFormResponseMRF = dbModel.Response;
                context.BuilderModel = builderModel;
                context.FieldRuleExecutor = fieldRuleExecutor;

                var validationContext = new FormValidationContext();

                foreach (var field in builderModel.Fields)
                {
                    validationContext.SetFieldContext(field.GetRenderId());
                    var driver = GetFormDriver(field.FieldTypeId);
                    await driver.Process(field, context, validationContext, response.Model);
                }

                return validationContext;
            }

            //TODO: Check validation
            var validationContext = await Process(userFormResponse, form.User);

            if (!validationContext.IsValid)
            {

            }

            if (userFormResponse.Response.Id == 0 && userFormResponse.Response.FormId != 0)
            {
                _db.UserFormResponses.Add(userFormResponse.Response);
            }

            return validationContext;
        }


        public string SerializeForm(UserFormResponse formResponse, FormBuilderModel formBuilder)
        {
            var formDictionary = new Dictionary<int, object>();
            foreach (var field in formBuilder.Fields)
            {
                var driver = GetFormDriver(field.FieldTypeId);
                var responseContext = new UserResponseContext(formResponse, field);
                var response = driver.BuildUserResponses(responseContext);
                if (response != null)
                {
                    formDictionary.TryAdd(field.Id, response);
                }
            }

            return JsonConvert.SerializeObject(formDictionary);
        }

        public string SerializeFormMRF(UserFormResponse formResponse, FormBuilderModel formBuilder, string UUID)
        {
            var formDictionary = new Dictionary<string, object>();
            var formDictionaryCustom = new Dictionary<string, object>();

            formDictionary.TryAdd("UUID", UUID);

            foreach (var field in formBuilder.Fields)
            {
                var driver = GetFormDriver(field.FieldTypeId);
                var responseContext = new UserResponseContext(formResponse, field);
                var response = driver.BuildUserResponses(responseContext);
                if (response != null)
                {
                    if (field.FieldTypeId == FieldTypeEnum.RadioButton)
                    {
                        var fieldOptionValue = _db.FieldOptions.FirstOrDefaultAsync(f => f.Id == Convert.ToInt32(response));

                        if (fieldOptionValue != null)
                        {
                            if (field.IsStandardField == true)
                            {
                                if (field.IsHidden == false)
                                {
                                    if (((GoRegister.ApplicationCore.Data.Models.SingleSelectField)field).SingleSelectType == SingleSelectField.SingleSelectTypeEnum.Radio)
                                    {
                                        formDictionary.TryAdd(field.DataTag, fieldOptionValue.Result.Description);
                                    }
                                    else
                                    {
                                        formDictionary.TryAdd(field.DataTag, fieldOptionValue.Result.AdditionalInformation);
                                    }
                                }
                                else
                                {
                                    fieldOptionValue.Result.AdditionalInformation = field.DefaultValue ?? "dummy";
                                    formDictionary.TryAdd(field.DataTag, fieldOptionValue.Result.AdditionalInformation);
                                }
                            }
                            else
                            {
                                if (((GoRegister.ApplicationCore.Data.Models.SingleSelectField)field).SingleSelectType == SingleSelectField.SingleSelectTypeEnum.Radio || ((GoRegister.ApplicationCore.Data.Models.SingleSelectField)field).SingleSelectType == SingleSelectField.SingleSelectTypeEnum.Select)
                                {
                                    formDictionaryCustom.TryAdd(field.Name, fieldOptionValue.Result.Description);
                                }
                                else
                                {
                                    formDictionaryCustom.TryAdd(field.Name, fieldOptionValue.Result.AdditionalInformation);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (field.DataTag == "DestinationExternalId")
                        {
                            string[] destination = response.ToString().Split("|^|");

                            if (field.IsStandardField == true)
                            {
                                formDictionary.TryAdd("DestinationExternalId", destination[0]);
                                formDictionary.TryAdd("DestinationFullAddress", destination[1]);
                            }
                            else
                            {
                                formDictionaryCustom.TryAdd("Destination External Id", destination[0]);
                                formDictionaryCustom.TryAdd("Destination Full Address", destination[1]);
                            }
                        }
                        else
                        {
                            if (field.IsStandardField == true)
                            {
                                if (field.DataTag == "CompanyName")
                                {
                                    formDictionary.TryAdd(field.DataTag, field.DefaultValue);
                                }
                                else if (field.IsHidden == true && field.DataTag != "ServicingCountry")
                                {
                                    formDictionary.TryAdd(field.DataTag, field.DefaultValue ?? "dummy");
                                }
                                else
                                {
                                    formDictionary.TryAdd(field.DataTag, response);
                                }
                            }
                            else
                            {
                                formDictionaryCustom.TryAdd(field.Name, response);
                            }
                        }
                    }
                }
                else
                {
                    if ((field.DataTag != "" && field.DataTag != null) && field.IsStandardField == true)
                    {
                        if (field.DataTag == "CompanyName")
                        {
                            formDictionary.TryAdd("CompanyName", field.DefaultValue);
                        }
                        else if (field.IsHidden == true)
                        {
                            formDictionary.TryAdd(field.DataTag, field.DefaultValue ?? "dummy");
                        }
                        else
                        {
                            formDictionary.TryAdd(field.DataTag, "");
                        }
                    }

                }

            }

            if (formDictionaryCustom.Values.Count > 0)
            {
                formDictionary.TryAdd("CustomFields", formDictionaryCustom);
            }

            return JsonConvert.SerializeObject(formDictionary);
        }


        public List<MRFFormDetails> SerializeFormMRFEmail(UserFormResponse formResponse, FormBuilderModel formBuilder, string UUID, bool isTPNCountry)
        {
            List<MRFFormDetails> mrfFormDetails = new List<MRFFormDetails>();

            foreach (var field in formBuilder.Fields)
            {
                var driver = GetFormDriver(field.FieldTypeId);
                var responseContext = new UserResponseContext(formResponse, field);
                var response = driver.BuildUserResponses(responseContext);

                MRFFormDetails mrfFormDetailsObj = new MRFFormDetails();

                if (response != null && field.IsHidden == false)
                {

                    if (field.FieldTypeId == FieldTypeEnum.RadioButton)
                    {
                        var fieldOptionValue = _db.FieldOptions.FirstOrDefaultAsync(f => f.Id == Convert.ToInt32(response));

                        if (fieldOptionValue != null)
                        {
                            mrfFormDetailsObj.FieldName = field.Name;
                            mrfFormDetailsObj.FieldValue = fieldOptionValue.Result.Description;
                        }
                    }
                    else if (field.FieldTypeId == FieldTypeEnum.MRFServicingCountry)
                    {
                        string countryName = String.Empty;

                        if (isTPNCountry)
                        {
                            countryName = _cache.GetCountry(response.ToString()).Result.Name;
                        }
                        else
                        {
                            countryName = _cache.GetCountryMRF(response.ToString()).Result.CountryName;
                        }

                        mrfFormDetailsObj.FieldName = field.Name;
                        mrfFormDetailsObj.FieldValue = countryName;

                    }
                    else if (field.FieldTypeId == FieldTypeEnum.MRFRequestorCountry)
                    {
                        string countryName = String.Empty;

                        /*if (isTPNCountry)
                        {
                            countryName = _cache.GetCountry(response.ToString()).Result.Name;
                        }
                        else
                        {
                            countryName = _cache.GetCountryMRF(response.ToString()).Result.CountryName;
                        }*/
                        countryName = _cache.GetCountry(response.ToString()).Result.Name;
                        mrfFormDetailsObj.FieldName = field.Name;
                        mrfFormDetailsObj.FieldValue = countryName;

                    }
                    else if (field.FieldTypeId == FieldTypeEnum.Date)
                    {
                        mrfFormDetailsObj.FieldName = field.Name;
                        mrfFormDetailsObj.FieldValue = Convert.ToDateTime(response).ToString("dd-MMM-yyyy");


                    }
                    else
                    {
                        if (field.DataTag == "DestinationExternalId")
                        {
                            string[] destination = response.ToString().Split("|^|");
                            mrfFormDetailsObj.FieldName = "Destination Full Address";
                            mrfFormDetailsObj.FieldValue = destination[1];
                        }
                        else if (field.DataTag == "CompanyName")
                        {
                            mrfFormDetailsObj.FieldName = field.Name;
                            mrfFormDetailsObj.FieldValue = field.DefaultValue;
                        }
                        //else if (field.IsStandardField == true && field.IsHidden == true)
                        //{
                        //    mrfFormDetailsObj.FieldName = field.Name;
                        //    mrfFormDetailsObj.FieldValue = field.DefaultValue ?? "dummy";
                        //}
                        else
                        {
                            mrfFormDetailsObj.FieldName = field.Name;
                            mrfFormDetailsObj.FieldValue = Convert.ToString(response);
                        }
                    }

                    mrfFormDetailsObj.FieldDataTag = field.DataTag;
                    mrfFormDetails.Add(mrfFormDetailsObj);

                }
                else if (field.IsHidden == false)
                {
                    if ((field.DataTag != "" && field.DataTag != null) && field.IsStandardField == true)
                    {
                        if (field.DataTag == "CompanyName")
                        {
                            mrfFormDetailsObj.FieldName = field.Name;
                            mrfFormDetailsObj.FieldValue = field.DefaultValue;
                        }
                        //else if (field.IsHidden == true)
                        //{
                        //    mrfFormDetailsObj.FieldName = field.Name;
                        //    mrfFormDetailsObj.FieldValue = field.DefaultValue ?? "dummy";
                        //}

                        mrfFormDetailsObj.FieldDataTag = field.DataTag;
                        mrfFormDetails.Add(mrfFormDetailsObj);
                    }
                }
                /*else if (field.IsHidden == true && field.FieldTypeId == FieldTypeEnum.MRFServicingCountry)
                {
                    string countryName = String.Empty;

                    if (isTPNCountry)
                    {
                        countryName = _cache.GetCountry(response.ToString()).Result.Name;
                    }
                    else
                    {
                        countryName = _cache.GetCountryMRF(response.ToString()).Result.CountryName;
                    }

                    mrfFormDetailsObj.FieldName = field.Name;
                    mrfFormDetailsObj.FieldValue = countryName;

                    mrfFormDetailsObj.FieldDataTag = field.DataTag;
                    mrfFormDetails.Add(mrfFormDetailsObj);
                }*/


            }

            return mrfFormDetails;
        }

        private IQueryable<Field> GetFieldsQuery()
        {
            return _db.Fields
                .Include(e => e.FieldOptions)//.ThenInclude("NextFieldOptionRules")
                .Include(e => e.RegistrationTypeFields)
                .Include(e => e.RegistrationPage)
                    .ThenInclude(p => p.Form)
                .Where(e => !e.IsDeleted)
                .OrderBy(e => e.SortOrder);
        }

        private IFormDriver GetFormDriver(FieldTypeEnum fieldType)
        {
            return GetFormDrivers()[fieldType];
        }

        private Dictionary<FieldTypeEnum, IFormDriver> GetFormDrivers()
        {
            return _formDrivers.ToDictionary(e => e.FieldType, e => e);
        }

        //private async Task<List<LookupOptionDto>> GetLanguagesAsync()
        //{
        //    return await _db.Language
        //        .Where(l => l.IsActive)
        //        .OrderBy(l => l.SortOrder)
        //        .Select(l => new LookupOptionDto { 
        //            Value = l.LanguageCode,
        //            Text = l.LanguageName                
        //        })
        //        .ToListAsync();
        //}
    }
}
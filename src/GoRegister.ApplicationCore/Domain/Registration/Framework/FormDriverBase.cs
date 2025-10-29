using AutoMapper;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Data.Enums;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using Newtonsoft.Json.Linq;
using Serilog;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework
{
    public abstract class FormDriverBase<TField, TFieldEditor, TResponse> : IFormDriver where TField : Field, new()
        where TFieldEditor : IFieldEditorModel, new()
    {
        public abstract FieldTypeEnum FieldType { get; }
        public abstract IFormResponseTranslator<TResponse> FormResponseTranslator { get; }
        public virtual Task<ResponseResult<TResponse>> GetExcelResponseAsync(TField field, ExcelRange excelRange)
        {
            return Task.FromResult(GetExcelResponse(field, excelRange));
        }
        public virtual ResponseResult<TResponse> GetExcelResponse(TField field, ExcelRange excelRange)
        {
            return null;
        }
        public abstract DataStorageStrategyEnum StorageStrategy { get; }

        // field editor properties
        protected virtual bool CanBeDeleted { get; } = true;
        public virtual bool IsForPresentation { get; } = false;
        protected virtual string EditorName { get; }
        protected virtual string EditorTemplate { get; }
        protected virtual string OverrideName { get; }
        protected virtual string DataTagFixed { get; }
        protected virtual bool IsUnique { get; }
        protected virtual List<FormType> FormTypes { get; set; } = new List<FormType>();

        public FieldEditorTypeModel GetEditorTypeModel()
        {
            var blankModel = new TFieldEditor();
            blankModel.SetFieldTypeId(FieldType);
            return new FieldEditorTypeModel()
            {
                CanBeDeleted = CanBeDeleted,
                IsForPresentation = IsForPresentation,
                FieldTypeId = FieldType,
                Name = string.IsNullOrWhiteSpace(EditorName) ? FieldType.ToString() : EditorName,
                Template = EditorTemplate,
                BlankField = blankModel,
                OverrideName = OverrideName,
                DataTagFixed = DataTagFixed,
                FormTypes = FormTypes,
                IsUnique = IsUnique
            };
        }

        public async Task<IFormDriverResult> Render(Field field, FormDisplayContext context, int projectId = 0)
        {
            var fieldContext = new FieldDisplayContext(field.Id, context);
            var result = await Display(field as TField, fieldContext, projectId);

            if (result == null) return null;

            // set pageid
            result.Model.PageId = field.GetPageRenderId();

            // build rules mapping
            // TODO: move out of reg service


            return result;
        }

        public abstract Task<IFormDriverResult> Display(TField field, FieldDisplayContext fieldDisplayContext,int projectId = 0);

        //public void Process(Field field, FormResponseContext context, FormValidationContext validationContext)
        //{
        //    //if (IsForPresentation) return;

        //    //var fieldModel = field as TField;
        //    //var val = FormResponseTranslator.Process(context.UpdateModel, field.Key);
        //    //var fieldContext = new FieldResponseContext(field.Id, context);

        //    //Validate(fieldModel, val, validationContext);
        //    //if(validationContext.IsValid)
        //    //    Process(fieldModel, val, fieldContext);
        //}

        public void Process(Field field, FormResponseContext formResponseContext, FormValidationContext validationContext)
        {
            //if (IsForPresentation) return;

            //var fieldModel = field as TField;
            //var val = FormResponseTranslator.Process(context.UpdateModel, field.Key);
            //var fieldContext = new FieldResponseContext(field.Id, context);

            //Validate(fieldModel, val, validationContext);
            //if(validationContext.IsValid)
            //    Process(fieldModel, val, fieldContext);
        }

        //public async Task Process(Field field, FormResponseContext context, FormValidationContext validationContext, Dictionary<string, JToken> form)
        //{
        //    // we shouldn't be processing presentation fields
        //    if (IsForPresentation) return;

        //    // readonly fields should only be changed from the admin
        //    if (field.IsReadOnly && !context.IsAdmin) return;

        //    // internal fields should only be changed from the admin
        //    if (field.IsHidden && !context.IsAdmin) return; 

        //    // check if field should be processed based on field rules
        //    // only processed for frontend submissions
        //    if (!context.IsAdmin && !context.FieldRuleExecutor.ShouldDisplayField(field.GetRenderId(), context.Response, form))
        //    {
        //        Log.Logger.Debug("Field: {FieldId} not to be displayed", field.Id);
        //        return;
        //    }

        //    var fieldModel = field as TField;
        //    var val = form.ContainsKey(fieldModel.GetRenderId())
        //        ? GetJsonResponse(fieldModel, form[fieldModel.GetRenderId()])
        //        : ResponseResult.Empty<TResponse>();

        //    var fieldContext = new FieldResponseContext(field.Id, context);
        //    await ValidateField(fieldModel, val, validationContext, fieldContext);
        //    if (validationContext.IsValid)
        //        Process(fieldModel, val, fieldContext);
        //}

        public async Task Process(Field field, FormResponseContext formResponseContext, FormValidationContext validationContext, Dictionary<string, JToken> form)
        {
            // we shouldn't be processing presentation fields
            if (IsForPresentation) return;

            // readonly fields should only be changed from the admin
            if (field.IsReadOnly && !formResponseContext.IsAdmin) return;

            // internal fields should only be changed from the admin
            if (field.IsHidden && !formResponseContext.IsAdmin) return;

            // check if field should be processed based on field rules
            // only processed for frontend submissions
            if (!formResponseContext.IsAdmin && !formResponseContext.FieldRuleExecutor.ShouldDisplayField(field.GetRenderId(), formResponseContext.UserFormResponseMRF, form))
            {
                Log.Logger.Debug("Field: {FieldId} not to be displayed", field.Id);
                return;
            }

            var fieldModel = field as TField;
            var val = form.ContainsKey(fieldModel.GetRenderId())
                ? GetJsonResponse(fieldModel, form[fieldModel.GetRenderId()])
                : ResponseResult.Empty<TResponse>();

            var fieldContext = new FieldResponseContext(field.Id, formResponseContext);
            await ValidateField(fieldModel, val, validationContext, fieldContext);
            if (validationContext.IsValid)
                Process(fieldModel, val, fieldContext);
        }

        protected virtual ResponseResult<TResponse> GetJsonResponse(TField field, JToken jsonValue)
        {
            // return empty result for null or empty json properties
            if (jsonValue.Type == JTokenType.Null || (jsonValue.Type == JTokenType.String && jsonValue.ToString() == string.Empty))
                return ResponseResult.Empty<TResponse>();

            return ResponseResult.Ok(jsonValue.ToObject<TResponse>());
        }

        //public async Task ValidateResponse(Field field, FormResponseContext context, FormValidationContext validationContext, ExcelRange excelRange)
        //{
        //    if (IsForPresentation) return;

        //    var fieldModel = field as TField;
        //    var val = await GetExcelResponseAsync(fieldModel, excelRange);
        //    if (val.Failed)
        //    {
        //        validationContext.AddError(val.Message);
        //        return;
        //    }

        //    var fieldContext = new FieldResponseContext(field.Id, context);
        //    await ValidateField(fieldModel, val, validationContext, fieldContext);
        //}

        public async Task ValidateResponse(Field field, FormResponseContext formResponseContext, FormValidationContext validationContext, ExcelRange excelRange)
        {
            if (IsForPresentation) return;

            var fieldModel = field as TField;
            var val = await GetExcelResponseAsync(fieldModel, excelRange);
            if (val.Failed)
            {
                validationContext.AddError(val.Message);
                return;
            }

            var fieldContext = new FieldResponseContext(field.Id, formResponseContext);
            await ValidateField(fieldModel, val, validationContext, fieldContext);
        }

        //public async Task Process(Field field, FormResponseContext context, FormValidationContext validationContext, ExcelRange excelRange)
        //{
        //    if (IsForPresentation) return;

        //    var fieldModel = field as TField;
        //    var val = await GetExcelResponseAsync(fieldModel, excelRange);
        //    if (val.Failed)
        //    {
        //        validationContext.AddError(val.Message);
        //        return;
        //    }

        //    var fieldContext = new FieldResponseContext(field.Id, context);
        //    await ValidateField(fieldModel, val, validationContext, fieldContext);
        //    if (validationContext.IsValid)
        //        Process(fieldModel, val, fieldContext);
        //}

        public async Task Process(Field field, FormResponseContext formResponseContextMRF, FormValidationContext validationContext, ExcelRange excelRange)
        {
            if (IsForPresentation) return;

            var fieldModel = field as TField;
            var val = await GetExcelResponseAsync(fieldModel, excelRange);
            if (val.Failed)
            {
                validationContext.AddError(val.Message);
                return;
            }

            var fieldContext = new FieldResponseContext(field.Id, formResponseContextMRF);
            await ValidateField(fieldModel, val, validationContext, fieldContext);
            if (validationContext.IsValid)
                Process(fieldModel, val, fieldContext);
        }

        //private async Task ValidateField(TField field, ResponseResult<TResponse> response, FormValidationContext context, FieldResponseContext responseContext)
        //{
        //    if (!responseContext.FormContext.SkipIsMandatoryCheck && field.IsMandatory)
        //    {
        //        if(!response.HasValue)
        //        {
        //            context.AddError($"{field.NameForValidation} is required");
        //            return;
        //        }

        //        if (typeof(TResponse) == typeof(string))
        //        {
        //            var stringValue = response.Value as string;
        //            if (stringValue == null || stringValue.Trim().Length == 0)
        //            {
        //                context.AddError($"{field.NameForValidation} is required");
        //                return;
        //            }
        //        }
        //    }

        //    await ValidateAsync(field, response, context, responseContext);
        //}

        private async Task ValidateField(TField field, ResponseResult<TResponse> responseMRF, FormValidationContext formValidationContextMRF, FieldResponseContext responseContext)
        {
            if (!responseContext.FormContext.SkipIsMandatoryCheck && field.IsMandatory)
            {
                if (!responseMRF.HasValue)
                {
                    formValidationContextMRF.AddError($"{field.NameForValidation} is required");
                    return;
                }

                if (typeof(TResponse) == typeof(string))
                {
                    var stringValue = responseMRF.Value as string;
                    if (stringValue == null || stringValue.Trim().Length == 0)
                    {
                        formValidationContextMRF.AddError($"{field.NameForValidation} is required");
                        return;
                    }
                }
            }

            await ValidateAsync(field, responseMRF, formValidationContextMRF, responseContext);
        }

        //public virtual Task ValidateAsync(TField field, ResponseResult<TResponse> response, FormValidationContext context, FieldResponseContext responseContext)
        //{
        //    return Task.CompletedTask;
        //}

        public virtual Task ValidateAsync(TField field, ResponseResult<TResponse> responseMRF, FormValidationContext formValidationContextMRF, FieldResponseContext responseContext)
        {
            return Task.CompletedTask;
        }

        protected abstract void Process(TField field, ResponseResult<TResponse> response, FieldResponseContext context);

        //public void Clear(TField field, FieldResponseContext context)
        //{

        //}

        public void Clear(TField field, FieldResponseContext fieldResponseContextMRF)
        {

        }

        //public virtual Task<object> GetCachedValueAsync(int fieldId, DelegateUserCacheGetContext context)
        //{
        //    return Task.FromResult(GetCachedValue(fieldId, context));
        //}

        public virtual Task<object> GetCachedValueAsync(int fieldId, DelegateUserCacheGetContext delegateUserCacheGetContextMRF)
        {
            return Task.FromResult(GetCachedValue(fieldId, delegateUserCacheGetContextMRF));
        }

        //public virtual object GetCachedValue(int fieldId, DelegateUserCacheGetContext context)
        //{
        //    return context.DelegateData.GetResponseValue(fieldId);
        //}

        public virtual object GetCachedValue(int fieldId, DelegateUserCacheGetContext delegateUserCacheGetContextMRF)
        {
            return delegateUserCacheGetContextMRF.DelegateData.GetResponseValue(fieldId);
        }

        //public virtual Task<object> BuildUserResponsesAsync(UserResponseContext context)
        //{
        //    return Task.FromResult(BuildUserResponses(context));
        //}

        public virtual Task<object> BuildUserResponsesAsync(UserResponseContext userResponseContextMRF)
        {
            return Task.FromResult(BuildUserResponses(userResponseContextMRF));
        }

        //public virtual object BuildUserResponses(UserResponseContext context)
        //{
        //    return null;
        //}

        public virtual object BuildUserResponses(UserResponseContext userResponseContextMRF)
        {
            return null;
        }

        public IFieldEditorModel BindEditorModel(JToken model)
        {
            return model.ToObject<TFieldEditor>();
        }

        public IFieldEditorModel BuildEditor(Field field, IMapper mapper)
        {
            field.FieldOptions = field.FieldOptions.OrderBy(e => e.SortOrder).ToList();

            var fieldModel = field as TField;
            var editViewModel = new TFieldEditor();

            mapper.Map(fieldModel, editViewModel);

            editViewModel.RegistrationTypes = field.RegistrationTypeFields.Select(e => e.RegistrationTypeId);
            //editViewModel.Rules = field.FieldOptionRules.Select(optionRule => new FieldRule
            //{
            //    Id = optionRule.Id.ToString(),
            //    FieldId = optionRule.FieldOption.FieldId.ToString(),
            //    OptionId = optionRule.FieldOptionId.ToString()
            //});

            Edit(fieldModel, editViewModel);
            return editViewModel;
        }

        protected virtual void Edit(TField field, TFieldEditor model) { }

        //public void UpdateEditor(Field field, IFieldEditorModel model, UpdateFormContext context)
        //{
        //    var fieldModel = field as TField;
        //    context.Mapper.Map(model, fieldModel);
        //    Update(fieldModel, (TFieldEditor)model, context);
        //}

        public void UpdateEditor(Field field, IFieldEditorModel model, UpdateFormContext updateFormContextMRF)
        {
            var fieldModel = field as TField;
            updateFormContextMRF.Mapper.Map(model, fieldModel);
            Update(fieldModel, (TFieldEditor)model, updateFormContextMRF);
        }

        //protected virtual void Update(TField field, TFieldEditor model, UpdateFormContext context) { }

        protected virtual void Update(TField field, TFieldEditor model, UpdateFormContext updateFormContextMRF) { }

        public Field CreateInstance() => new TField();

        //public Task<string> GetSummaryValueAsync(Field field, DelegateUserCacheGetContext context)
        //{
        //    var tField = field as TField;
        //    return GetSummaryValueAsync(tField, context);
        //}

        public Task<string> GetSummaryValueAsync(Field field, DelegateUserCacheGetContext delegateUserCacheGetContextMRF)
        {
            var tField = field as TField;
            return GetSummaryValueAsync(tField, delegateUserCacheGetContextMRF);
        }

        //protected virtual async Task<string> GetSummaryValueAsync(TField field, DelegateUserCacheGetContext context)
        //{
        //    var val = await GetCachedValueAsync(field.Id, context);
        //    return val != null ? val.ToString() : null;
        //}

        protected virtual async Task<string> GetSummaryValueAsync(TField field, DelegateUserCacheGetContext delegateUserCacheGetContextMRF)
        {
            var val = await GetCachedValueAsync(field.Id, delegateUserCacheGetContextMRF);
            return val != null ? val.ToString() : null;
        }
    }

    public abstract class FormDriverBase<TField, TResponse> : FormDriverBase<TField, FieldEditorModel, TResponse> where TField : Field, new()
    {

    }


    //public abstract class FormDriverBase<TDisplay, TResponse> : IFormDriver where TDisplay : IFieldDisplayModel, new()
    //{
    //    public abstract FieldTypeEnum FieldType { get; }
    //    public abstract IFormResponseTranslator<TResponse> FormResponseTranslator { get; }

    //    public IFieldDisplayModel Build(Field dbField)
    //    {
    //        return Build2(dbField);
    //    }

    //    public TDisplay Build2(Field field)
    //    {
    //        var displayModel = new TDisplay();
    //        var config = new MapperConfiguration(cfg => cfg.CreateMap<Field, TDisplay>());
    //        config.CreateMapper().Map(field, displayModel);
    //        return displayModel;
    //    }

    //    public Task<IFormDriverResult> Render(Field field)
    //    {
    //        var displayModel = Build2(field);
    //        return Display(displayModel);
    //    }


    //    public abstract Task<IFormDriverResult> Display(TDisplay field);

    //    public void Process(Field field, FieldResponseContext context)
    //    {
    //        var fieldModel = Build2(field);
    //        var val = FormResponseTranslator.Process(context.UpdateModel, fieldModel.Key);
    //        Validate(fieldModel, val, context);
    //        Process(fieldModel, val, context);

    //    }
    //    public abstract void Validate(TDisplay field, TResponse response, FieldResponseContext context);

    //    public abstract void Process(TDisplay field, TResponse response, FieldResponseContext context);
    //}
}

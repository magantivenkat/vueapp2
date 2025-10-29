using AutoMapper;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.BuildTests.Domain.BulkUpload.Services.Fakes
{
    //public abstract class BaseFakeFieldDriver : IFormDriver
    //{
    //    public abstract FieldTypeEnum FieldType { get; }

    //    public DataStorageStrategyEnum StorageStrategy => throw new NotImplementedException();

    //    public bool IsForPresentation => throw new NotImplementedException();

    //    public IFieldEditorModel BindEditorModel(JToken model)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IFieldEditorModel BuildEditor(Field field, IMapper mapper)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public object BuildUserResponses(UserResponseContext context)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<object> BuildUserResponsesAsync(UserResponseContext context)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Field CreateInstance()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<object> GetCachedValueAsync(DelegateUserCacheGetContext context)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public FieldEditorTypeModel GetEditorTypeModel()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Process(Field field, FormResponseContext context, FormValidationContext validationContext)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task Process(Field field, FormResponseContext context, FormValidationContext validationContext, ExcelRange excelRange)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task Process(Field field, FormResponseContext context, FormValidationContext validationContext, Dictionary<string, JToken> form)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<IFormDriverResult> Render(Field field, FormDisplayContext context)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void UpdateEditor(Field field, IFieldEditorModel model, UpdateFormContext context)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task ValidateResponse(Field field, FormResponseContext context, FormValidationContext validationContext, ExcelRange excelRange)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}

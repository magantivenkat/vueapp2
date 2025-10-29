using GoRegister.ApplicationCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Framework.Domain;
using OfficeOpenXml;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using Newtonsoft.Json.Linq;
using AutoMapper;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework
{
    public interface IFormDriver
    {
        FieldTypeEnum FieldType { get; }
        Task<IFormDriverResult> Render(Field field, FormDisplayContext context, int projectId = 0);
        void Process(Field field, FormResponseContext context, FormValidationContext validationContext);
        Task Process(Field field, FormResponseContext context, FormValidationContext validationContext, ExcelRange excelRange);
        Task Process(Field field, FormResponseContext context, FormValidationContext validationContext, Dictionary<string, JToken> form);
        DataStorageStrategyEnum StorageStrategy { get; }
        bool IsForPresentation { get; }
        Task<object> GetCachedValueAsync(int fieldId, DelegateUserCacheGetContext context);
        object BuildUserResponses(UserResponseContext context);
        Task<object> BuildUserResponsesAsync(UserResponseContext context);
        IFieldEditorModel BindEditorModel(JToken model);
        void UpdateEditor(Field field, IFieldEditorModel model, UpdateFormContext context);
        Field CreateInstance();
        FieldEditorTypeModel GetEditorTypeModel();
        IFieldEditorModel BuildEditor(Field field, IMapper mapper);
        Task ValidateResponse(Field field, FormResponseContext context, FormValidationContext validationContext, ExcelRange excelRange);
        Task<string> GetSummaryValueAsync(Field field, DelegateUserCacheGetContext context);
    }
}

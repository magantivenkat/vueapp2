using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Translators;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data.Enums;
using OfficeOpenXml;
using GoRegister.ApplicationCore.Domain.Registration.Fields.Text;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.LastName
{
    public class LastNameFieldFormDriver : InputFormDriverBase<LastNameField, string, TextFieldDisplayModel>
    {
        protected override string EditorName => "Last Name";
        protected override string OverrideName => "Last Name";
        protected override string DataTagFixed => "LastName";
        public override FieldTypeEnum FieldType => FieldTypeEnum.LastName;
        protected override bool IsUnique => true;

        public override IFormResponseTranslator<string> FormResponseTranslator => new StringFormResponseTranslator();

        public override DataStorageStrategyEnum StorageStrategy => DataStorageStrategyEnum.String;

        //public override Task<TextFieldDisplayModel> Display(TextFieldDisplayModel model, LastNameField field, FieldDisplayContext context)
        //{
        //    model.Type = "text-input";
        //    return Task.FromResult(model);
        //}

        public override Task<TextFieldDisplayModel> Display(TextFieldDisplayModel model, LastNameField field, FieldDisplayContext fieldDisplayContextMRF, int projectId = 0)
        {
            model.Type = "text-input";
            return Task.FromResult(model);
        }

        //protected override void Process(LastNameField field, ResponseResult<string> response, FieldResponseContext context)
        //{
        //    if(response.HasValue)
        //        context.FormContext.Response.DelegateUser.UpdateLastName(response.Value);
        //}

        protected override void Process(LastNameField field, ResponseResult<string> responseMRF, FieldResponseContext fieldResponseContextMRF)
        {
            if (responseMRF.HasValue)
                fieldResponseContextMRF.FormContext.UserFormResponseMRF.DelegateUser.UpdateLastName(responseMRF.Value);
        }

        public override ResponseResult<string> GetExcelResponse(LastNameField field, ExcelRange excelRange)
        {
            if (string.IsNullOrWhiteSpace(excelRange.Text))
            {
                return ResponseResult.Empty<string>();
            }

            return ResponseResult.Ok(excelRange.Text?.Trim());
        }

        //public override object BuildUserResponses(UserResponseContext context)
        //{
        //    return context.Response.DelegateUser.ApplicationUser.LastName;
        //}
        public override object BuildUserResponses(UserResponseContext userResponseContextMRF)
        {
            return userResponseContextMRF.UserFormResponseMRF.DelegateUser.ApplicationUser.LastName;
        }

        //protected override Task<string> GetSummaryValueAsync(LastNameField field, DelegateUserCacheGetContext context)
        //{
        //    return Task.FromResult(context.DelegateData.LastName);
        //}

        protected override Task<string> GetSummaryValueAsync(LastNameField field, DelegateUserCacheGetContext delegateUserCacheGetContextMRF)
        {
            return Task.FromResult(delegateUserCacheGetContextMRF.DelegateData.LastName);
        }
    }
}

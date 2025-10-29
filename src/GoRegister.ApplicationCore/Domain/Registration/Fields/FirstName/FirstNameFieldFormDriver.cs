using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Translators;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data.Enums;
using OfficeOpenXml;
using GoRegister.ApplicationCore.Domain.Registration.Fields.Text;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.FirstName
{
    public class FirstNameFieldFormDriver : InputFormDriverBase<FirstNameField, string, TextFieldDisplayModel>
    {
        protected override string OverrideName => "First Name";
        protected override string EditorName => "First Name";
        protected override string DataTagFixed => "FirstName";
        public override FieldTypeEnum FieldType => FieldTypeEnum.FirstName;
        protected override bool IsUnique => true;

        public override IFormResponseTranslator<string> FormResponseTranslator => new StringFormResponseTranslator();

        public override DataStorageStrategyEnum StorageStrategy => DataStorageStrategyEnum.String;

        public override Task<TextFieldDisplayModel> Display(TextFieldDisplayModel model, FirstNameField field, FieldDisplayContext context, int projectId = 0)
        {
            model.Type = "text-input";
            return Task.FromResult(model);
        }

        //protected override void Process(FirstNameField field, ResponseResult<string> response, FieldResponseContext context)
        //{
        //    if(response.HasValue)
        //        context.FormContext.Response.DelegateUser?.UpdateFirstName(response.Value);
        //}

        protected override void Process(FirstNameField field, ResponseResult<string> responseMRF, FieldResponseContext fieldResponseContextMRF)
        {
            if (responseMRF.HasValue)
                fieldResponseContextMRF.FormContext.UserFormResponseMRF.DelegateUser?.UpdateFirstName(responseMRF.Value);
        }

        public override ResponseResult<string> GetExcelResponse(FirstNameField field, ExcelRange excelRange)
        {
            if (string.IsNullOrWhiteSpace(excelRange.Text))
            {
                return ResponseResult.Empty<string>();
            }

            return ResponseResult.Ok(excelRange.Text?.Trim());
        }

        //public override object BuildUserResponses(UserResponseContext context)
        //{
        //    return context.Response.DelegateUser.ApplicationUser.FirstName;
        //}

        public override object BuildUserResponses(UserResponseContext userResponseContextMRF)
        {
            return userResponseContextMRF.UserFormResponseMRF.DelegateUser.ApplicationUser.FirstName;
        }

        //protected override Task<string> GetSummaryValueAsync(FirstNameField field, DelegateUserCacheGetContext context)
        //{
        //    return Task.FromResult(context.DelegateData.FirstName);
        //}

        protected override Task<string> GetSummaryValueAsync(FirstNameField field, DelegateUserCacheGetContext delegateUserCacheGetContextMRF)
        {
            return Task.FromResult(delegateUserCacheGetContextMRF.DelegateData.FirstName);
        }
    }
}

using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Translators;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data.Enums;
using OfficeOpenXml;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using System.Text.RegularExpressions;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.Text
{
    public class TextFieldFormDriver : InputFormDriverBase<TextField, TextFieldEditorModel, string, TextFieldDisplayModel>
    {
        public override FieldTypeEnum FieldType => FieldTypeEnum.Textbox;

        public override IFormResponseTranslator<string> FormResponseTranslator => new StringFormResponseTranslator();

        public override DataStorageStrategyEnum StorageStrategy => DataStorageStrategyEnum.String;
        protected override string EditorTemplate => "TextEdit";

        public override Task<TextFieldDisplayModel> Display(TextFieldDisplayModel model, TextField field, FieldDisplayContext context, int projectId = 0)
        {
            model.Type = "text-input";
            model.Placeholder = field.Placeholder;

            if (field.InputType == "email")
            {
                model.AddValidation("email", true);
                model.InputType = "email";
            }

            return Task.FromResult(model);
        }

        protected override void Process(TextField field, ResponseResult<string> response, FieldResponseContext context)
        {
            if (response.HasValue)
                context.SetStringValue(response.Value);
            else
                context.ClearStringValue();
        }

        public override ResponseResult<string> GetExcelResponse(TextField field, ExcelRange excelRange)
        {
            if (string.IsNullOrWhiteSpace(excelRange.Text))
            {
                return ResponseResult.Empty<string>();
            }

            return ResponseResult.Ok(excelRange.Text?.Trim());
        }

        //public override object BuildUserResponses(UserResponseContext context)
        //{
        //    var foResult = context.Response.GetStringValue(context.FieldId);
        //    if (foResult)
        //        return foResult.Value;

        //    return null;
        //}

        public override object BuildUserResponses(UserResponseContext userResponseContextMRF)
        {
            var foResult = userResponseContextMRF.UserFormResponseMRF.GetStringValue(userResponseContextMRF.FieldId);
            if (foResult)
                return foResult.Value;

            return null;
        }

        //public async override Task ValidateAsync(TextField field, ResponseResult<string> response, FormValidationContext context, FieldResponseContext responseContext)
        //{
        //    if(response.HasValue)
        //    {
        //        if (field.InputType == "email")
        //        {
        //            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        //            Match match = regex.Match(response.Value);

        //            if (!match.Success)
        //            {
        //                context.AddError("Please provide a valid email address");
        //                return;
        //            }

        //        }
        //    }            
        //}

        public async override Task ValidateAsync(TextField field, ResponseResult<string> responseMRF, FormValidationContext formValidationContextMRF, FieldResponseContext responseContext)
        {
            if (responseMRF.HasValue)
            {
                if (field.InputType == "email")
                {
                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regex.Match(responseMRF.Value);

                    if (!match.Success)
                    {
                        formValidationContextMRF.AddError("Please provide a valid email address");
                        return;
                    }

                }
            }
        }
    }
}

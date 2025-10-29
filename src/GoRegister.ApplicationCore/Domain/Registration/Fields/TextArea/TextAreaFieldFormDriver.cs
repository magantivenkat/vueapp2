using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Translators;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.TextArea
{
    public class TextAreaFieldFormDriver : InputFormDriverBase<TextAreaField, TextAreaFieldEditorModel, string, TextAreaFieldDisplayModel>
    {
        public override FieldTypeEnum FieldType => FieldTypeEnum.TextSquare;

        public override IFormResponseTranslator<string> FormResponseTranslator => new StringFormResponseTranslator();

        public override DataStorageStrategyEnum StorageStrategy => DataStorageStrategyEnum.String;
        protected override string EditorTemplate => "TextAreaEdit";

        public override Task<TextAreaFieldDisplayModel> Display(TextAreaFieldDisplayModel model, TextAreaField field, FieldDisplayContext context, int projectId = 0)
        {
            model.Type = "text-area";
            model.Placeholder = field.Placeholder;

            return Task.FromResult(model);
        }

        protected override void Process(TextAreaField field, ResponseResult<string> response, FieldResponseContext context)
        {
            if (response.HasValue)
                context.SetStringValue(response.Value);
            else
                context.ClearStringValue();
        }

        public override ResponseResult<string> GetExcelResponse(TextAreaField field, ExcelRange excelRange)
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
    }
}

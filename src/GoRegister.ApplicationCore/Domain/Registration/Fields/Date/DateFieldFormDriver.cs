using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Translators;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data.Enums;
using System;
using OfficeOpenXml;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Models;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.Date
{
    public class DateFieldFormDriver : InputFormDriverBase<DateField, DateFieldEditorModel, DateTime, DateFieldDisplayModel>

    {
        public override FieldTypeEnum FieldType => FieldTypeEnum.Date;
        public override IFormResponseTranslator<DateTime> FormResponseTranslator => new DateTimeFormResponseTranslator();
        protected override string EditorTemplate => "DateEdit";

        public override DataStorageStrategyEnum StorageStrategy => DataStorageStrategyEnum.DateTime;

        public override Task<DateFieldDisplayModel> Display(DateFieldDisplayModel model, DateField field, FieldDisplayContext context, int projectId = 0)
        {
            model.Type = "date";
            model.PickerType = field.PickerType;
            return Task.FromResult(model);
        }

        protected override void Process(DateField field, ResponseResult<DateTime> response, FieldResponseContext context)
        {
            if (response.HasValue)
                context.SetDateTimeValue(response.Value);
        }

        public override ResponseResult<DateTime> GetExcelResponse(DateField field, ExcelRange excelRange)
        {
            if (excelRange.Value is DateTime)
            {
                return ResponseResult.Ok(excelRange.GetValue<DateTime>());
            }

            var text = excelRange.Text?.Trim();
            if (string.IsNullOrWhiteSpace(text)) return ResponseResult.Empty<DateTime>();

            return ResponseResult.Fail<DateTime>();
        }

        //public override object BuildUserResponses(UserResponseContext context)
        //{
        //    var foResult = context.Response.GetDateTimeValue(context.FieldId);
        //    if (foResult)
        //        return foResult.Value;

        //    return null;
        //}

        public override object BuildUserResponses(UserResponseContext userResponseContextMRF)
        {
            var foResult = userResponseContextMRF.UserFormResponseMRF.GetDateTimeValue(userResponseContextMRF.FieldId);
            if (foResult)
                return foResult.Value;

            return null;
        }

        //protected override Task<string> GetSummaryValueAsync(DateField field, DelegateUserCacheGetContext context)
        //{
        //    var ob = context.DelegateData.GetResponseValue(field.Id);
        //    if (ob == null) return Task.FromResult<string>(null);
        //    var dateTime = (DateTime)ob;
        //    string formatted;
        //    switch (field.PickerType)
        //    {
        //        case DateFieldDisplayType.DateAndTime:
        //            formatted = dateTime.ToString("dddd, dd MMMM yyyy HH:mm");
        //            break;
        //        case DateFieldDisplayType.Date:
        //        default:
        //            formatted = dateTime.ToString("dddd, dd MMMM yyyy");
        //            break;
        //    }

        //    return Task.FromResult(formatted);
        //}

        protected override Task<string> GetSummaryValueAsync(DateField field, DelegateUserCacheGetContext delegateUserCacheGetContextMRF)
        {
            var ob = delegateUserCacheGetContextMRF.DelegateData.GetResponseValue(field.Id);
            if (ob == null) return Task.FromResult<string>(null);
            var dateTime = (DateTime)ob;
            string formatted;
            switch (field.PickerType)
            {
                case DateFieldDisplayType.DateAndTime:
                    formatted = dateTime.ToString("dddd, dd MMMM yyyy HH:mm");
                    break;
                case DateFieldDisplayType.Date:
                default:
                    formatted = dateTime.ToString("dddd, dd MMMM yyyy");
                    break;
            }

            return Task.FromResult(formatted);
        }
    }
}

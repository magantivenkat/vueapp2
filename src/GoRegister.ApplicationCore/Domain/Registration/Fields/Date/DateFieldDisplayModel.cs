using GoRegister.ApplicationCore.Data.Enums;
using System;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.Date
{
    public class DateFieldDisplayModel : BaseSingleFieldDisplayModel
    {
        public DateTime? Value { get; set; }
        public string ValueHtmlFormatted => Value?.ToString("yyyy-MM-dd");
        public DateFieldDisplayType PickerType { get; set; }
    }
}

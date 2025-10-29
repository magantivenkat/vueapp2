using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.Date
{
    public class DateFieldEditorModel : FieldEditorModel
    {
        public DateFieldDisplayType PickerType { get; set; }
    }
}

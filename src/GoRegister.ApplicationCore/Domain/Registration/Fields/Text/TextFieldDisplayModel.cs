using GoRegister.ApplicationCore.Domain.Registration.Framework;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.Text
{
    public class TextFieldDisplayModel : BaseSingleFieldDisplayModel, IFieldDisplayModel<string>
    {
        public string Value { get; set; }
        public string Placeholder { get; set; }
        public string InputType { get; set; } = "text";
    }
}
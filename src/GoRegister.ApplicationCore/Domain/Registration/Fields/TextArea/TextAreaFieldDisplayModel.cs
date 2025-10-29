using GoRegister.ApplicationCore.Domain.Registration.Framework;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.TextArea
{
   public class TextAreaFieldDisplayModel :  BaseSingleFieldDisplayModel, IFieldDisplayModel<string>
    {
        public string Value { get; set; }
        public string Placeholder { get; set; }
       
    }
}

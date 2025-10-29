using GoRegister.ApplicationCore.Domain.Registration.Framework.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields.Text
{
    public class TextFieldEditorModel : FieldEditorModel
    {
        public string Placeholder { get; set; }
        public string InputType { get; set; } = "text";
    }
}

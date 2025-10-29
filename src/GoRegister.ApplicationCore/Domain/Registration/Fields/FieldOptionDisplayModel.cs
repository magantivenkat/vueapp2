using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields
{
    public class FieldOptionDisplayModel
    {
        public FieldOptionDisplayModel(string value) : this(value, value) { }

        public FieldOptionDisplayModel(int value, string text) : this(value.ToString(), text) { }

        public FieldOptionDisplayModel(string value, string text)
        {
            Value = value;
            Text = text;
        }

        public string Value { get; set; }
        public string Text { get; set; }
        public bool Disabled { get;set; }

        public string HtmlId => $"opt-{Value}";
    }
}

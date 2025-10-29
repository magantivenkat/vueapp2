using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.ProjectThemes.Variables
{
    public interface ICssVariable
    {
        string Key { get; }
        string Type { get; }
        string Label { get; set; }
        string DefaultValue { get; set; }
        void Build(BuildCssVariableContext context);
    }

    public abstract class CssVariableBase : ICssVariable
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public string DefaultValue { get; set; }

        private string _variable = null;
        public string Variable
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_variable))
                    return ToCssVariable(Key);

                return _variable;
            }
            set
            {
                _variable = value;
            }
        }

        public abstract string Type { get; }

        public abstract void Build(BuildCssVariableContext context);

        private string ToCssVariable(string input)
        {
            return "--" + string.Concat(
                (input ?? string.Empty)
                    .Select(
                        (x, i) => i > 0 && char.IsUpper(x) && !char.IsUpper(input[i - 1])
                            ? $"-{x}"
                            : x.ToString()
                    )
                ).ToLower();
        }
    }

    public class BasicCssVariable : CssVariableBase
    {
        public override string Type => "Basic";

        public BasicCssVariable(string key, string label, string defaultValue = null)
        {
            Key = key;
            Label = label;
            DefaultValue = defaultValue;
        }

        public override void Build(BuildCssVariableContext context)
        {
            if (context.Values.ContainsKey(Key))
                context.Add(Variable, context.Values[Key]);
        }
    }

    public class FontCssVariable : CssVariableBase
    {
        public override string Type => "Font";
        const string fontBase = "-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,'Helvetica Neue',Arial,'Noto Sans','Liberation Sans',sans-serif,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol','Noto Color Emoji'";

        public FontCssVariable(string key, string label)
        {
            Key = key;
            Label = label;
            DefaultValue = "--font-family-sans-serif";
        }

        public override void Build(BuildCssVariableContext context)
        {
            if (context.Values.ContainsKey(Key))
            {
                var val = context.Values[Key];
                if (val == DefaultValue) return;
                context.Add(Variable, $"'{val}',{fontBase}");
            }
        }
    }

    public class BuildCssVariableContext
    {
        private Dictionary<string, string> _variables = new Dictionary<string, string>();

        public BuildCssVariableContext(Dictionary<string, string> values)
        {
            Values = values;
        }

        public Dictionary<string, string> Values { get; }

        public void Add(string variable, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _variables[variable] = value;
            }
        }

        public string CreateVariablesCSS()
        {
            var sb = new StringBuilder();
            foreach (var variable in _variables)
            {
                sb.Append($"{variable.Key}:{variable.Value};");
            }
            return sb.ToString();
        }
    }
}

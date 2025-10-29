using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework
{
    public class FormValidationContext
    {
        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();
        private string _key = "";

        public bool IsValid => !_errors.Any();

        public Dictionary<string, string> Errors => _errors;

        public void SetFieldContext(string key)
        {
            _key = key;
        }

        public void AddError(string errorMessage)
        {
            _errors[_key] = errorMessage;
        }

        public void AddError(string key, string errorMessage)
        {
            _errors[key] = errorMessage;
        }
    }
}

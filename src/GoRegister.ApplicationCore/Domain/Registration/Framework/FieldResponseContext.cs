using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework
{
    public class FieldResponseContext
    {
        private readonly int _fieldId;

        public FieldResponseContext(int fieldId, FormResponseContext formContext)
        {
            _fieldId = fieldId;
            FormContext = formContext;
        }

        public FormResponseContext FormContext { get; }

        public void SetStringValue(string value)
        {
            //FormContext.Response.SetStringValue(_fieldId, value);
            FormContext.UserFormResponseMRF.SetStringValue(_fieldId, value);
        }

        public void ClearStringValue()
        {
            //FormContext.Response.ClearStringValue(_fieldId);
            FormContext.UserFormResponseMRF.ClearStringValue(_fieldId);
        }

        public void SetDateTimeValue(DateTime value)
        {
            //FormContext.Response.SetDateTimeValue(_fieldId, value);
            FormContext.UserFormResponseMRF.SetDateTimeValue(_fieldId, value);
        }

        public void ClearDateTimeValue()
        {
            //FormContext.Response.ClearDateTimeValue(_fieldId);
            FormContext.UserFormResponseMRF.ClearDateTimeValue(_fieldId);
        }

        public void SetFieldOptionId(int value)
        {
            //FormContext.Response.SetFieldOptionId(_fieldId, value);
            FormContext.UserFormResponseMRF.SetFieldOptionId(_fieldId, value);
        }

        public void ClearFieldOptionId()
        {
            //FormContext.Response.ClearFieldOptionId(_fieldId);
            FormContext.UserFormResponseMRF.ClearFieldOptionId(_fieldId);
        }

        public void ClearResponse()
        {
            //FormContext.Response.ClearResponse(_fieldId);
            FormContext.UserFormResponseMRF.ClearResponse(_fieldId);
        }
    }

    public class FormResponseContext
    {
        //public UserFormResponse Response { get; set; }
        public UserFormResponse UserFormResponseMRF { get; set; }
        public IUpdateModel UpdateModel { get; set; }
        public FormBuilderModel BuilderModel { get; set; }
        public FieldRuleExecutor FieldRuleExecutor { get; set; }
        public bool IsAdmin { get; set; }
        public bool SkipIsMandatoryCheck { get; set; }
        public FormExecutionFrom FormExecutionFrom { get; set; } = FormExecutionFrom.Form;
    }

    public enum FormExecutionFrom
    {
        Form,
        BulkUpload
    }

    public class FieldRuleExecutor
    {
        private readonly IEnumerable<FormRuleModel> _rules;
        private readonly Dictionary<FieldTypeEnum, IFormDriver> _ruleDrivers;

        public FieldRuleExecutor(Dictionary<FieldTypeEnum, IFormDriver> ruleDrivers, IEnumerable<FormRuleModel> rules)
        {
            _rules = rules;
            _ruleDrivers = ruleDrivers;
        }

        public bool ShouldDisplayField(string fieldId, UserFormResponse formResponse, Dictionary<string, JToken> form)
        {
            var nextFieldModelId = fieldId;
            var rulesForField = _rules.Where(rule => rule.NextFieldId == nextFieldModelId);

            if (!rulesForField.Any()) return true;

            foreach (var rule in rulesForField)
            {
                if (_rules.Any(e => e.NextFieldId == rule.FieldId))
                {
                    var shouldDisplayParentField = ShouldDisplayField(rule.FieldId, formResponse, form);
                    if (!shouldDisplayParentField)
                        continue;
                }

                var result = form != null && form.ContainsKey(rule.FieldId)
                    ? ResponseResult.Ok(form[rule.FieldId].ToObject<int>())
                    : ResponseResult.Fail<int>();

                int value;
                if (!result)
                {
                    var dbResult = formResponse.GetFieldOptionId(int.Parse(rule.FieldId));
                    if (!dbResult)
                    {
                        return false;
                    }

                    value = dbResult.Value;
                }
                else
                {
                    value = result.Value;
                }

                // TODO: this string/int id mess needs to be solved...
                if (rule.FieldOptionId == value.ToString()) return true;

                continue;
            }

            return false;

            //driver.MatchesRule()
        }
    }
}

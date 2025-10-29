using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Registration.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Fields
{
    public abstract class BaseSingleFieldDisplayModel : IFieldDisplayModel
    {
        public string Id { get; set; }
        public bool IsForPresentation => false;
        public FieldTypeEnum FieldTypeId { get; set; }
        public string Key { get; set; }
        public string PageId { get; set; }
        public string Type { get; set; }
        public virtual string SummaryTemplate { get; set; } = "TextValueSummary";
        public string DataTag { get; set; }
        public string Label { get; set; }
        public string HelpTextBefore { get; set; }
        public string HelpTextAfter { get; set; }
        public string ValidationName { get; set; }
        public bool Readonly { get; set; }
        public int Cols { get; set; }
        public Dictionary<string, object> Validations { get; } = new Dictionary<string, object>();
        public IEnumerable<FieldRule> Rules { get; set; }

        public void AddValidation(string key, object value) => Validations.TryAdd(key, value);

        public bool? AllowTPNCountries { get; set; }
        public void Build(Field field)
        {
            Id = field.GetRenderId();
            Key = field.GetRenderId();
            Cols = field.Cols;
            ValidationName = field.ValidationName;
            HelpTextAfter = field.HelpTextAfter;
            HelpTextBefore = field.HelpTextBefore;
            Label = field.Name;
            FieldTypeId = field.FieldTypeId;
            Readonly = field.IsReadOnly;
            AllowTPNCountries = field.AllowTPNCountries;
            DataTag = field.DataTag;    
        }
    }
}

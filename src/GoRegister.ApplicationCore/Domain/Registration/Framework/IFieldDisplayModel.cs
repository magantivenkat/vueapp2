using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework
{
    public interface IFieldDisplayModel
    {
        string Id { get; set; }
        string Key { get; }
        FieldTypeEnum FieldTypeId { get; set; }
        void Build(Field field);
        Dictionary<string, object> Validations { get; }
        IEnumerable<FieldRule> Rules { get; set; }
        string Type { get; set; }
        string SummaryTemplate { get; set; }
        bool IsForPresentation { get; }
        string PageId { get; set; }
    }

    public class FieldRule
    {
        public string Id { get; set; }
        public IEnumerable<string> Values { get; set; }
    }

    public interface IFieldDisplayModel<T> : IFieldDisplayModel
    {
        T Value { get; set; }
    }
}

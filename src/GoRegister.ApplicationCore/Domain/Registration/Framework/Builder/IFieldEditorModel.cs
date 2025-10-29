using AutoMapper;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework.Builder
{
    public interface IFieldEditorModel
    {
        string Id { get; set; }
        bool IsDeleted { get; set; }
        int SortOrder { get; set; }
        bool TryGetId(out int id);
        string Template { get; set; }
        FieldTypeEnum FieldTypeId { get; set; }
        IEnumerable<FieldRuleEditorModel> Rules { get; set; }
        IEnumerable<int> RegistrationTypes { get; set; }
        void SetFieldTypeId(FieldTypeEnum fieldTypeId);
    }

    public class FieldEditorModel : IFieldEditorModel
    {
        public bool TryGetId(out int id)
        {
            return int.TryParse(Id, out id);
        }

        public void SetFieldTypeId(FieldTypeEnum fieldType)
        {
            FieldTypeId = fieldType;
        }

        public string Template { get; set; }
        public FieldTypeEnum FieldTypeId { get; set; }
        public string Id { get; set; }

        public string Name { get; set; }
        public int SortOrder { get; set; }
        public IEnumerable<int> RegistrationTypes { get; set; } = new List<int>();

        public bool IsDeleted { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsForPresentation { get; set; }
        public bool IsHidden { get; set; }
        public IEnumerable<FieldRuleEditorModel> Rules { get; set; } = new List<FieldRuleEditorModel>();

        public bool IsStandardField { get; set; }
        // base field settings
        public string DataTag { get; set; }
        public string HelpTextBefore { get; set; }
        public string HelpTextAfter { get; set; }
        public string ReportingHeader { get; set; }
        public string ValidationName { get; set; }
        public int Cols { get; set; } = 12;
        public bool IsNew { get; set; }
        public bool? AllowTPNCountries { get; set; }
    }

    public class FieldRuleEditorModel
    {
        public string FieldId { get; set; }
        public IEnumerable<string> OptionIds { get; set; }
    }

    public class UpdateFormContext
    {
        public readonly IMapper Mapper;
        public UpdateFormContext(IMapper mapper)
        {
            Mapper = mapper;
        }

        public Dictionary<string, FieldMap> FieldMaps { get; } = new Dictionary<string, FieldMap>();
        public Dictionary<string, FieldOption> OptionMaps { get; } = new Dictionary<string, FieldOption>();

        public void AddFieldMap(string id, Field field, IFieldEditorModel updateModel)
        {
            FieldMaps.Add(id, new FieldMap { Field = field, EditField = updateModel });
        }

        public void AddOptionMap(string id, FieldOption option)
        {
            OptionMaps.Add(id, option);
        }

        public class FieldMap
        {
            public Field Field { get; set; }
            public IFieldEditorModel EditField { get; set; }
        }

        public List<Func<Task>> PreSaveExecuteActions { get; } = new List<Func<Task>>();
    }
}

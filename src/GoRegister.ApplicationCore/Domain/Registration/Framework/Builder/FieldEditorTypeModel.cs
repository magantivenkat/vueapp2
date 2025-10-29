using GoRegister.ApplicationCore.Data.Enums;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework.Builder
{
    public class FieldEditorTypeModel
    {
        public FieldTypeEnum FieldTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFieldEditorModel BlankField { get; set; }
        public string Template { get; set; }
        public string OverrideName { get; set; }
        public bool IsUnique { get; set; }
        public bool IsForPresentation { get; set; }
        public bool CanBeDeleted { get; set; } = true;
        public string DataTagFixed { get; set; }
        public List<FormType> FormTypes { get; set; }
    }
}

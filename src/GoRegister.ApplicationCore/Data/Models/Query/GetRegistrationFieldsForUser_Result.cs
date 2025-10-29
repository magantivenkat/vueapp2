using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Data.Models.Query
{
    public partial class GetRegistrationFieldsForUser_Result
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int RegistrationPageId { get; set; }
        public string Name { get; set; }
        public int FieldTypeId { get; set; }
        public bool IsMandatory { get; set; }
        public int SortOrder { get; set; }
        public string Options { get; set; }
        public string DataTag { get; set; }
        public Nullable<int> MinLength { get; set; }
        public Nullable<int> MaxLength { get; set; }
        public bool CanModify { get; set; }
        public bool IsReadOnly { get; set; }
        public string DefaultValue { get; set; }
        public string HelpTextToolTip { get; set; }
        public string Placeholder { get; set; }
        public System.Guid UniqueIdentifier { get; set; }
        public string Class { get; set; }
        public string CustomAttributes { get; set; }
        public string SubText { get; set; }
        public string ApiFieldName { get; set; }
        public string ReportingHeader { get; set; }
        public bool IsDeleted { get; set; }
        public string PreText { get; set; }
        public bool IsInternalOnly { get; set; }
        public bool IsHidden { get; set; }
        public string UserFieldValue { get; set; }
        public string UserFieldValueDescription { get; set; }
        public Nullable<int> UserFieldOptionId { get; set; }
        public bool IsForPresentation { get; set; }
        public bool AllowTPNCountries { get; set; }
    }
}

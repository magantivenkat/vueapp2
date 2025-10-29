using System;

namespace GoRegister.ApplicationCore.Data.Models.Query
{
    public partial class GetRegistrationFieldDropdownOptionsForUser_Result
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public string Description { get; set; }
        public Nullable<int> Capacity { get; set; }
        public int SortOrder { get; set; }
        public string DataTag { get; set; }
        public string ReportingTitle { get; set; }
        public Nullable<int> AgendaId { get; set; }
        public string Attributes { get; set; }
        public string Class { get; set; }
        public string InternalInformation { get; set; }
        public string AdditionalInformation { get; set; }
        public Nullable<decimal> VisaAllowance { get; set; }
        public bool IsDeleted { get; set; }
        public string AdditionalInformation1 { get; set; }
        public string AdditionalInformation2 { get; set; }
    }
}

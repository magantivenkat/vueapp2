using GoRegister.ApplicationCore.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoRegister.ApplicationCore.Domain.BulkUpload.Models
{
    public class BulkUploadMappingModel
    {
        public int EmailFieldId { get; set; }
        public BulkUploadRegistrationTypeStatus RegistrationTypeStatus { get; set; } = BulkUploadRegistrationTypeStatus.UseDefault;
        public int? RegistrationTypeColumnIndex { get; set; }
        public int? RegistrationTypeId { get; set; }
        public List<HeaderMapping> HeaderMappings { get; set; } = new List<HeaderMapping>();
        public string UploadId { get; set; }
    }

    public class BulkUploadMappingViewModel
    {
        public IEnumerable<BulkUploadField> Fields { get; set; }
        public IEnumerable<TextValueModel> RegistrationTypes { get; set; }
    }

    public class HeaderMapping
    {
        public int ColumnIndex { get; set; }
        public string ColumnName { get; set; }
        public int? FieldId { get; set; }
        public string Property { get; set; }
    }

    public class BulkUploadField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DataTag { get; set; }
    }

    public enum BulkUploadRegistrationTypeStatus
    {
        UseDefault,
        UseFromUpload,
        PleaseSelect
    }
}

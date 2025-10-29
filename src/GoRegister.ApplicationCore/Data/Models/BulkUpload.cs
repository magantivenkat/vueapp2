using GoRegister.ApplicationCore.Data.Enums;
using System;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class BulkUpload : MustHaveProjectEntity
    {
        public Guid Id { get; set; }
        public BulkUploadStatusEnum Status { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public DateTime? DateUploadedUtc { get; set; }
        public string FilePath { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}

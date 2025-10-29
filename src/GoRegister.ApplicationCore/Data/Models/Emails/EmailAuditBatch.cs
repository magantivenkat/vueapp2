using GoRegister.ApplicationCore.Data.Enums;
using System;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class EmailAuditBatch : MustHaveProjectEntity
    {
        public int Id { get; set; }
        public Guid BatchId { get; set; }
        public int CreatedByUserId { get; set; }
        public int EmailCount { get; set; }
        public int EmailIssueCount { get; set; }
        public EmailAuditBatchStatus Status { get;set;}
        public DateTime DateCreatedUtc { get; set; }
        public DateTime? DateSentUtc { get; set; }
        public Email Email { get; set; }

        public ApplicationUser CreatedByUser { get; set; }
        public ICollection<EmailAudit> EmailAudits { get; set; }
    }
}

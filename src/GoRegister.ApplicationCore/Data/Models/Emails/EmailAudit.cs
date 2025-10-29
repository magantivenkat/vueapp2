using GoRegister.ApplicationCore.Data.Enums;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class EmailAudit
    {
        public int Id { get; set; }
        public ICollection<EmailAuditNotification> EmailAuditNotifications { get; set; }
        public Guid BatchId { get; set; }
        public int? DelegateUserId { get; set; }
        public EmailAuditStatus StatusId { get; set; }
        public string FromEmail { get; set; }
        public string FromEmailDisplayName { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public int? EmailAuditBatchId { get; set; }
        public int? EmailTemplateId { get; set; }
        public DateTime DateCreatedUtc { get; set; }

        public EmailAuditBatch EmailAuditBatch { get; set; }
        public EmailTemplate EmailTemplate { get; set; }
        public DelegateUser DelegateUser { get; set; }
    }

    public class EmailAuditMap : IEntityTypeConfiguration<EmailAudit>
    {
        public void Configure(EntityTypeBuilder<EmailAudit> builder)
        {
            builder.HasOne(ea => ea.DelegateUser)
                .WithMany()
               .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey(e => e.DelegateUserId);
        }
    }
}
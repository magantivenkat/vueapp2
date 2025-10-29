using GoRegister.ApplicationCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class DelegateUserAudit : MustHaveProjectEntity
    {
        public int Id { get; set; }
        [ForeignKey("DelegateUser")]
        public int UserId { get; set; }
        public int? ActionById { get; set; }
        public ActionedFrom ActionedFrom { get; set; }
        public DateTime ActionedUtc { get; set; } = SystemTime.UtcNow;
        public int? RegistrationStatusId { get; set; }
        public int? RegistrationTypeId { get; set; }
        public string RegistrationTypeName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public int? UserFormResponseId { get; set; }

        public UserFormResponse UserFormResponse { get; set; }
        public DelegateUser DelegateUser { get; set; }

        public int ActionedById { get; set; }
        public ApplicationUser ActionedBy { get; set; }

        public RegistrationType RegistrationType { get; set; }
        public ICollection<UserFieldResponseAudit> UserFieldResponseAudits { get; set; } = new HashSet<UserFieldResponseAudit>();
    }

    public class DelegateUserAuditMap : IEntityTypeConfiguration<DelegateUserAudit>
    {
        public void Configure(EntityTypeBuilder<DelegateUserAudit> builder)
        {

            builder.HasOne(dua => dua.DelegateUser)
                    .WithMany(dua => dua.Audits)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey(e => e.UserId);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class UserFieldResponseAudit : IMustHaveProject
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey("DelegateUser")]
        public int UserId { get; set; }
        public int FieldId { get; set; }
        public int? FieldOptionId { get; set; }
        public string StringValue { get; set; }
        public int? NumberValue { get; set; }
        public bool? BooleanValue { get; set; }
        public DateTime? DateTimeValue { get; set; }

        public virtual Field Field { get; set; }
        public virtual FieldOption FieldOption { get; set; }
        public virtual Project Project { get; set; }
        public virtual DelegateUser DelegateUser { get; set; }

    }

    public class UserFieldResponseAuditMap : IEntityTypeConfiguration<UserFieldResponseAudit>
    {
        public void Configure(EntityTypeBuilder<UserFieldResponseAudit> builder)
        {
            builder
                .HasOne(ufra => ufra.Field)
                .WithMany(ufra => ufra.UserFieldResponseAudits)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

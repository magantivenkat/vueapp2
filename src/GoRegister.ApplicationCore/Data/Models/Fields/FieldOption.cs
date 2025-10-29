namespace GoRegister.ApplicationCore.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FieldOption : MustHaveProjectEntity
    {
        public FieldOption()
        {
            this.FieldOptionRules = new HashSet<FieldOptionRule>();
            this.NextFieldOptionRules = new HashSet<FieldOptionRule>();
        }
    
        public int Id { get; set; }
        public int FieldId { get; set; }
        public Field Field { get; set; }
        public string Description { get; set; }
        public Nullable<int> Capacity { get; set; }
        public int SortOrder { get; set; }
        public string DataTag { get; set; }
        public string ReportingTitle { get; set; }
        public string Attributes { get; set; }
        public string Class { get; set; }
        public bool IsHidden { get; set; }
        public string InternalInformation { get; set; }
        public string AdditionalInformation { get; set; }
        public bool IsDeleted { get; set; }
        public string AdditionalInformation1 { get; set; }
        public string AdditionalInformation2 { get; set; }

        [InverseProperty("FieldOption")]
        public virtual ICollection<FieldOptionRule> FieldOptionRules { get; set; }

        [InverseProperty("NextFieldOption")]
        public virtual ICollection<FieldOptionRule> NextFieldOptionRules { get; set; }
        public ICollection<UserFieldResponseAudit> UserFieldResponseAudits { get; set; }
        public ICollection<UserFieldResponse> UserFieldResponses { get; set; }



        [NotMapped]
        public string TempId { get; private set; }
        public void SetTempId(string id)
        {
            TempId = id;
        }

        public string GetRenderId()
        {
            return Id == 0 ? TempId : Id.ToString();
        }

    }

    public class FieldOptionMap : IEntityTypeConfiguration<FieldOption>
    {
        public void Configure(EntityTypeBuilder<FieldOption> builder)
        {
            builder
                .HasOne(fo => fo.Field)
                .WithMany(f => f.FieldOptions)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

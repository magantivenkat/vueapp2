using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class UserFieldResponse : IMustHaveProject
    {
        public UserFieldResponse()
        {

        }

        public UserFieldResponse(int fieldId)
        {
            FieldId = fieldId;
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int FieldId { get; set; }
        public int? FieldOptionId { get; set; }
        public string StringValue { get; set; }
        public int? NumberValue { get; set; }
        public bool? BooleanValue { get; set; }
        public string CountryId { get; set; }
        public int UserFormResponseId { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime? DateModified { get; set; }

        public virtual Field Field { get; set; }
        public virtual FieldOption FieldOption { get; set; }
        public virtual Project Project { get; set; }
        public virtual Country Country { get; set; }
        public UserFormResponse UserFormResponse { get; set; }
    }

    public class UserFieldResponseMap : IEntityTypeConfiguration<UserFieldResponse>
    {
        public void Configure(EntityTypeBuilder<UserFieldResponse> builder)
        {
            builder
                .HasOne(ufra => ufra.Field)
                .WithMany(ufra => ufra.UserFieldResponses)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class EmailTemplateRegistrationType : MustHaveProjectEntity
    {
        public int EmailTemplateId { get; set; }
        public int RegistrationTypeId { get; set; }

        public EmailTemplate EmailTemplate { get; set; }
        public RegistrationType RegistrationType { get; set; }
    }

    public class EmailTemplateRegistrationTypeMap : IEntityTypeConfiguration<EmailTemplateRegistrationType>
    {
        public void Configure(EntityTypeBuilder<EmailTemplateRegistrationType> builder)
        {
            builder.HasKey(e => new { e.EmailTemplateId, e.RegistrationTypeId });
        }
    }
}

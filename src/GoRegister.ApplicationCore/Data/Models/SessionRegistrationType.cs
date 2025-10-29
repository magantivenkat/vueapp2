using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class SessionRegistrationType
    {
        public int SessionId { get; set; }
        public Session Session { get; set; }
        public int RegistrationTypeId { get; set; }
        public RegistrationType RegistrationType { get; set; }
    }

    public class SessionRegistrationTypeMap : IEntityTypeConfiguration<SessionRegistrationType>
    {
        public void Configure(EntityTypeBuilder<SessionRegistrationType> builder)
        {
            builder.HasKey(srt => new { srt.SessionId, srt.RegistrationTypeId });
        }
    }

}

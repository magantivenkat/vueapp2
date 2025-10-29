using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class SessionFieldCategory
    {
        public int SessionFieldId { get; set; }
        public SessionField SessionField { get; set; }
        public int SessionCategoryId { get; set; }
        public SessionCategory SessionCategory { get; set; }
    }

    public class SessionFieldCategoryMap : IEntityTypeConfiguration<SessionFieldCategory>
    {
        public void Configure(EntityTypeBuilder<SessionFieldCategory> builder)
        {
            builder.HasKey(srt => new { srt.SessionFieldId, srt.SessionCategoryId });
        }
    }
}

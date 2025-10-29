using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class UserProjectMapping
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public ApplicationUser User { get; set; }
    }

    public class UserProjectMappingMap : IEntityTypeConfiguration<UserProjectMapping>
    {
        public void Configure(EntityTypeBuilder<UserProjectMapping> builder)
        {
            builder.HasKey(upm => new { upm.ProjectId, upm.UserId });

            builder.HasOne(us => us.User)
                    .WithMany(usr => usr.UserProjectMapping)
                    .HasForeignKey(fk=>fk.UserId);

            builder.HasOne(us => us.Project)
                   .WithMany(usr => usr.UserProjectMapping)
                   .HasForeignKey(fk => fk.ProjectId);
        }
    }
}

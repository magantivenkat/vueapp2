using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class UserRole : IdentityRole<int>
    {

    }

    public class UserRoleMap : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(model => model.Id);

            builder.HasData(
                new UserRole { Id = 2, Name = "DigitalServices", NormalizedName = "DIGITALSERVICES", ConcurrencyStamp = "5c537e38-9b4d-4354-89da-fab33dd47c28" }
            );

            builder.HasData(
                new UserRole { Id = 3, Name = "MeetingPlanner", NormalizedName = "MEETINGPLANNER", ConcurrencyStamp = "b4ed5c30-2ad6-40e7-86f0-79369ccbcf2c" }
            );
        }
    }
}

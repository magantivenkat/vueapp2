using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoRegister.ApplicationCore.Data.Models
{
    public class MenuItemRegistrationType : MustHaveProjectEntity
    {
        public int RegistrationTypeId { get; set; }
        public RegistrationType RegistrationType { get; set; }
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
    }

    public class MenuItemRegistrationTypeMap : IEntityTypeConfiguration<MenuItemRegistrationType>
    {
        public void Configure(EntityTypeBuilder<MenuItemRegistrationType> builder)
        {
            builder.HasKey(model => new { model.MenuItemId, model.RegistrationTypeId });
        }
    }
}

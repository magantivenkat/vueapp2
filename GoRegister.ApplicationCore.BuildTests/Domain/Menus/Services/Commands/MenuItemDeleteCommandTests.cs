using FluentAssertions;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Menus.Commands;
using GoRegister.TestingCore;
using Xunit;

namespace GoRegister.ApplicationCore.BuildTests.Domain.Menus.Services.Commands
{
    public class MenuItemDeleteCommandTests : DatabaseContextTest
    {
        public MenuItemDeleteCommandTests()
        {
            
        }

        [Fact]
        public async void Handle_Result_Delete_Menu()
        {
            using (var db = GetDatabase())
            {
                db.MenuItems.Add(new MenuItem {  Id = 100, ProjectId = 2, Description = "Test123", MenuItemType = MenuItemType.Register, Order = 4, OpenInNewTab = false, CssClass = "Test123", CustomPageId = 1, FormType = null, FormId = null, Fragment = "Test" });
                db.SaveChanges();
                db.MenuItemRegistrationTypes.Add(new MenuItemRegistrationType { RegistrationTypeId = 2, MenuItemId = 100, ProjectId = 2 });
                db.SaveChanges();
            }
            using (var db = GetDatabase())
            {
                var command = new MenuItemDeleteCommand { Id = 100};
                var sut = new MenuItemDeleteCommand.Handler(db);
                var result = await sut.Handle(command, new System.Threading.CancellationToken());
            }

            using (var db = GetDatabase())
            {
                var menuItem = db.MenuItems.Find(100);
                menuItem.Should().BeNull();

                var menuItemregtype = db.MenuItemRegistrationTypes.Find(100, 2);
                menuItemregtype.Should().BeNull();
            }

        }
    }
}

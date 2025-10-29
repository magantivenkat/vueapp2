using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Delegates.Models;
using GoRegister.ApplicationCore.Domain.Menus;
using GoRegister.ApplicationCore.Domain.Menus.Models;
using System;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.BuildTests.Domain.Menus.Fakes
{
    public class LinkHandlerFake : IMenuHandler
    {
        public MenuItemType MenuItemType => MenuItemType.Link;

        public Task ProcessMenuItem(BuildMenuItemModel menuItem, MenuLinkModel displayModel)
        {
            throw new NotImplementedException();
        }

        public bool ShouldDisplay(BuildMenuItemModel menuItem, AttendeeAuthorizationModel attendee)
        {
            throw new NotImplementedException();
        }
    }
}

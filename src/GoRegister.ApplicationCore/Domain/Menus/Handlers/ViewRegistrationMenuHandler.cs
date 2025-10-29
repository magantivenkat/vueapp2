using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Delegates.Models;
using GoRegister.ApplicationCore.Domain.Menus.Models;
using GoRegister.ApplicationCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Menus.Handlers
{
    public class ViewRegistrationMenuHandler : IMenuHandler
    {
        private readonly IUrlHelper _urlHelper;

        public ViewRegistrationMenuHandler(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public MenuItemType MenuItemType => MenuItemType.ViewRegistration;

        public Task ProcessMenuItem(BuildMenuItemModel menuItem, MenuLinkModel displayModel)
        {
            displayModel.Link = _urlHelper.ActionLink("Index", "Register", fragment: menuItem.Fragment);
            return Task.CompletedTask;
        }

        public bool ShouldDisplay(BuildMenuItemModel menuItem, AttendeeAuthorizationModel attendee)
        {
            if (attendee.IsAnonymous) return false;
            if (attendee.RegistrationStatus == RegistrationStatus.NotInvited || attendee.RegistrationStatus == RegistrationStatus.Invited) return false;
            return true;
        }
    }
}

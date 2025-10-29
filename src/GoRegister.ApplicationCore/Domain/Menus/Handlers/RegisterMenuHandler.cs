using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Delegates.Models;
using GoRegister.ApplicationCore.Domain.Menus.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GoRegister.ApplicationCore.Domain.Menus.Handlers
{
    public class RegisterMenuHandler : IMenuHandler
    {
        private readonly IUrlHelper _urlHelper;

        public RegisterMenuHandler(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public MenuItemType MenuItemType => MenuItemType.Register;

        public Task ProcessMenuItem(BuildMenuItemModel menuItem, MenuLinkModel displayModel)
        {
            displayModel.Link = _urlHelper.ActionLink("Index", "Register", fragment: menuItem.Fragment);
            return Task.CompletedTask;
        }

        public bool ShouldDisplay(BuildMenuItemModel menuItem, AttendeeAuthorizationModel attendee)
        {
            if (attendee.RegistrationStatus == RegistrationStatus.Invited) return true;
            return false;
        }
    }
}

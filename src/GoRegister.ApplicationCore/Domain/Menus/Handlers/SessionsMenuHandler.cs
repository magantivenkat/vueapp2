using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Delegates.Models;
using GoRegister.ApplicationCore.Domain.Menus.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Menus.Handlers
{
    class SessionsMenuHandler : IMenuHandler
    {
        private readonly IUrlHelper _urlHelper;

        public SessionsMenuHandler(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public MenuItemType MenuItemType => MenuItemType.Sessions;

        public Task ProcessMenuItem(BuildMenuItemModel menuItem, MenuLinkModel displayModel)
        {
            displayModel.Link = _urlHelper.ActionLink("Index", "Sessions", fragment: menuItem.Fragment);
            return Task.CompletedTask;
        }

        public bool ShouldDisplay(BuildMenuItemModel menuItem, AttendeeAuthorizationModel attendee)
        {            
            if (attendee.RegistrationStatus == RegistrationStatus.Confirmed) return true;
            return false;
        }
    }
}

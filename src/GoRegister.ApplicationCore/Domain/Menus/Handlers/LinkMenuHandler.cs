using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Delegates;
using GoRegister.ApplicationCore.Domain.Delegates.Models;
using GoRegister.ApplicationCore.Domain.Menus.Models;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Menus.Handlers
{
    public class LinkMenuHandler : IMenuHandler
    {
        private readonly IAttendeeAuthorizationService _attendeeAuthorizationService;

        public LinkMenuHandler(IAttendeeAuthorizationService attendeeAuthorizationService)
        {
            _attendeeAuthorizationService = attendeeAuthorizationService;
        }

        public MenuItemType MenuItemType => MenuItemType.Link;

        public Task ProcessMenuItem(BuildMenuItemModel menuItem, MenuLinkModel displayModel)
        {
            displayModel.Link = $"{menuItem.AnchorLink}{(!string.IsNullOrWhiteSpace(menuItem.Fragment) ? $"#{menuItem.Fragment}" : "")}";
            return Task.CompletedTask;
        }

        public bool ShouldDisplay(BuildMenuItemModel menuItem, AttendeeAuthorizationModel attendee)
        {
            var authModel = new ContentAuthorizationModel
            {
                AllowAnonymous = false,
                RegistrationStatuses = menuItem.RegistrationStatuses,
                RegistrationTypes = menuItem.RegistrationTypeIds
            };

            return _attendeeAuthorizationService.Authorize(attendee, authModel);
        }
    }
}

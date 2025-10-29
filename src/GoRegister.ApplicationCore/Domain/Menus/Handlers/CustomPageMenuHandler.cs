using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.CustomPages.Services;
using GoRegister.ApplicationCore.Domain.Delegates;
using GoRegister.ApplicationCore.Domain.Delegates.Models;
using GoRegister.ApplicationCore.Domain.Menus.Models;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Domain.Menus.Handlers
{
    public class CustomPageMenuHandler : IMenuHandler
    {
        private readonly IAttendeeAuthorizationService _attendeeAuthorizationService;
        private readonly ICustomPageService _customPageService;

        public CustomPageMenuHandler(IAttendeeAuthorizationService attendeeAuthorizationService, ICustomPageService customPageService)
        {
            _attendeeAuthorizationService = attendeeAuthorizationService;
            _customPageService = customPageService;
        }

        public MenuItemType MenuItemType => MenuItemType.CustomPage;

        public bool ShouldDisplay(BuildMenuItemModel menuItem, AttendeeAuthorizationModel attendee)
        {
            var authModel = new ContentAuthorizationModel
            {
                AllowAnonymous = false,
                RegistrationStatuses = menuItem.CustomPageRegistrationStatuses,
                RegistrationTypes = menuItem.CustomPageRegistrationTypes
            };

            return _attendeeAuthorizationService.Authorize(attendee, authModel);
        }

        public Task ProcessMenuItem(BuildMenuItemModel menuItem, MenuLinkModel displayModel)
        {
            if (string.IsNullOrWhiteSpace(displayModel.Label))
            {
                displayModel.Label = menuItem.CustomPageTitle;
            }

            displayModel.Link = _customPageService.GetPathForCustomPage(menuItem.CustomPageSlug, menuItem.CustomPageType.Value, menuItem.Fragment);

            return Task.CompletedTask;
        }
    }
}

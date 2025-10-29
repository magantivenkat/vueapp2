using GoRegister.ApplicationCore.Extensions;
using GoRegister.Areas.Admin.Extensions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.AdminUsers;
using GoRegister.ApplicationCore.Domain.CustomPages.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.ViewComponents
{
    public class CustomPageAuditsViewComponent : ViewComponent
    {
        private readonly ICustomPageService _customPageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAdminUserService _adminUserService;

        public CustomPageAuditsViewComponent(ICustomPageService customPageService,
            IAdminUserService adminUserService,
            UserManager<ApplicationUser> userManager)
        {
            _customPageService = customPageService;
            _adminUserService = adminUserService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int customPageAuditId)
        {
            var model = await _customPageService.GetAuditAsync(customPageAuditId);

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var adminUserSettings = await _adminUserService.GetUserSettingsAsync(user.Id);

            if (adminUserSettings == null) return View(model);

            var profileTimeZone = adminUserSettings.TimeZone;
            var profileDateFormat = adminUserSettings.DateFormat;

            foreach (var customPage in model)
            {
                customPage.TimeStampFormat = customPage.TimeStamp.SetUserProfileDateTimeFormat(profileTimeZone, profileDateFormat);

                customPage.ToolTipAuditInfo =
                    $"This was changed by {user.FirstName} {user.LastName} at {customPage.TimeStampFormat}";

                customPage.HumanizedDateSent = customPage.TimeStamp.HumanizeDateTime(profileTimeZone);
            }

            return View(model);
        }

    }
}

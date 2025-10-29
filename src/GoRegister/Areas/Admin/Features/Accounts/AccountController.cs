using GoRegister.ApplicationCore.Extensions;
using GoRegister.Areas.Admin.Controllers;
using GoRegister.Areas.Admin.Extensions;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.AdminUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Features.Accounts
{
    public class AccountController : AdminControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IAdminUserService _adminUserService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IAdminUserService adminUserService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _adminUserService = adminUserService;
        }

        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                // what should we do here?
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");

            return View();
        }

        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var adminUserSettings = await _adminUserService.GetUserSettingsAsync(user.Id);

            if (adminUserSettings == null)
            {
                return NotFound();
            }

            adminUserSettings.TimeZones.GetTimeZoneList();

            adminUserSettings.DateFormats.GetDateTimeFormatList();

            return View(adminUserSettings);
        }

        [HttpPost]
        public async Task<IActionResult> Settings(AccountSettingsModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _adminUserService.UpdateUserSettingsAsync(model);

            if (result.Succeeded)
            {
                //Settings was updated
                ViewBag.SuccessMessage = "Successfully saved.";

                model.TimeZones.GetTimeZoneList();

                model.DateFormats.GetDateTimeFormatList();

                return View(model);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Settings", error.Description);
            }


            return View(model);
        }
    }
}

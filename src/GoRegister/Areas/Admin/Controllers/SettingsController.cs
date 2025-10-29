using Microsoft.AspNetCore.Mvc;
using GoRegister.ApplicationCore.Domain.Settings.Models;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using GoRegister.ApplicationCore.Extensions;
using Microsoft.Extensions.Configuration;
using GoRegister.ApplicationCore.Data.Enums;
using GoRegister.ApplicationCore.Domain.Projects.Configuration;
using GoRegister.ApplicationCore.Domain.AdminUsers;

namespace GoRegister.Areas.Admin.Controllers
{
    public class SettingsController : ProjectAdminControllerBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IConfiguration _configuration;
        private readonly IAdminUserService _adminUserService;

        public SettingsController(
            ISettingsService settingsService,
              IConfiguration configuration,
              IAdminUserService adminUserService
        )
        {
            _settingsService = settingsService;
            _configuration = configuration;
            _adminUserService = adminUserService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _settingsService.GetSettingsAsync<GeneralSettingsModel>();
            var emailSettings = _configuration.GetSection("ProjectEmailNotification").Get<ProjectEmailNotificationConfiguration>();

            if (model.EmailAddress.Contains(emailSettings.NotificationSuffix))
            {
                model.EmailType = ProjectEmailType.CustomEmail.ToString();
                model.CustomEmailAddress = model.EmailAddress.Substring(0, model.EmailAddress.IndexOf("@"));
                model.EmailAddress = "";
            }
            else
            {
                model.SelectedEmail = model.EmailAddress;
                model.EmailType = ProjectEmailType.ClientEmail.ToString();
            }

            model.TimeZones.GetTimeZoneList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(GeneralSettingsModel model)
        {
            if (ModelState.IsValid)
            {
                var emailSettings = _configuration.GetSection("ProjectEmailNotification").Get<ProjectEmailNotificationConfiguration>();

                if (model.EmailType.ToLower() == ProjectEmailType.CustomEmail.ToString().ToLower())
                {
                    model.EmailAddress = model.CustomEmailAddress + emailSettings.NotificationSuffix;
                }

                await _settingsService.UpdateSettings(model);
                model.TimeZones.GetTimeZoneList();

                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Seo()
        {
            var model = await _settingsService.GetSettingsAsync<SeoSettingsModel>();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Seo(SeoSettingsModel model)
        {
            if (ModelState.IsValid)
            {
                await _settingsService.UpdateSettings(model);
                return View();
            }

            return View();
        }

        public async Task<IActionResult> Password()
        {
            var model = await _settingsService.GetSettingsAsync<PasswordSettingsModel>();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Password(PasswordSettingsModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SitewidePasswordPlainText != null && !String.IsNullOrEmpty(model.SitewidePasswordPlainText.Trim()))
                    model.SitewidePasswordHashed = new PasswordHasher<ApplicationUser>().HashPassword(null, model.SitewidePasswordPlainText);
                await _settingsService.UpdateSettings(model);
                return View();
            }

            return View();
        }

        public async Task<IActionResult> Share()
        {
            var projModel = await _settingsService.GetSettingsAsync<GeneralSettingsModel>();
            var Model = await _adminUserService.GetAllMeetingPlannerUsers(projModel.Id);
            Model.ProjectId = projModel.Id;

            return View("ShareUser", Model);
        }

        [HttpPost]
        public async Task<IActionResult> Share(int userId, int projectId)
        {
            var Model = await _adminUserService.SaveUserProjectMapping(userId, projectId);
            if (Model != null)
                return PartialView("_UserMappedProj", Model);
            else
                return View();
        }
        
        public async Task<IActionResult> DeleteMapping(int userId)
        {
            var projModel = await _settingsService.GetSettingsAsync<GeneralSettingsModel>();
            var Model = await _adminUserService.DeleteUserProjectMapping(userId,projModel.Id);
            if (Model != null)
                return View("ShareUser", Model);
            else
                return View();

        }
    }
}

using GoRegister.ApplicationCore.Extensions;
using GoRegister.Areas.Admin.Models.ChartViewModels;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.AdminUsers;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using GoRegister.ApplicationCore.Domain.Settings.Models;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.ViewComponents
{
    //[ViewComponent(Name = "ChartJsLine")]
    public class ChartJsLineViewComponent : ViewComponent
    {
        private readonly IDelegateService _delegateService;
        private readonly ISettingsService _settingsService;
        private readonly IAdminUserService _adminUserService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChartJsLineViewComponent(IDelegateService delegateService,
            ISettingsService settingsService,
            IAdminUserService adminUserService,
            UserManager<ApplicationUser> userManager)
        {
            _delegateService = delegateService;
            _settingsService = settingsService;
            _adminUserService = adminUserService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string dateRange, string header, string labelDataSets)
        {
            var delegates = await _delegateService.GetList();

            var registeredUsers = delegates
                .Where(delegateUser =>
                    delegateUser.RegistrationStatus == ApplicationCore.Data.Enums.RegistrationStatus.Confirmed.ToString() &&
                    !delegateUser.IsTest)
                .ToList();

            var firstConfirmedRegistration = registeredUsers.OrderBy(r => r.ConfirmedUtc).FirstOrDefault();

            if (firstConfirmedRegistration == null)
            {
                return View(new ChartLineViewModel { Header = header, Labels = new List<string>(), DataSetsLabel = labelDataSets, ChartData = new List<int>() });
            }

            var projectSettings = await _settingsService.GetSettingsAsync<GeneralSettingsModel>();

            var labels = new List<string>();
            var chartData = new List<int>();

            var user = await _userManager.GetUserAsync((ClaimsPrincipal)User);

            var adminUserSettings = await _adminUserService.GetUserSettingsAsync(user.Id);

            var profileTimeZone = adminUserSettings.TimeZone;
            var profileDateFormat = adminUserSettings.DateFormat;

            var compareDate = firstConfirmedRegistration.ConfirmedUtc ?? new System.DateTime();

            if (dateRange == "Day")
            {
                while (compareDate < projectSettings.StartDate)
                {
                    var chartProfileDateFormat = compareDate.SetUserProfileShortDateFormat(profileTimeZone, profileDateFormat);

                    labels.Add(chartProfileDateFormat);

                    var registrations = registeredUsers.Count(u => u.ConfirmedUtc.Value.Year == compareDate.Year &&
                                                                   u.ConfirmedUtc.Value.Month == compareDate.Month &&
                                                                   u.ConfirmedUtc.Value.Day == compareDate.Day);

                    chartData.Add(registrations);

                    compareDate = compareDate.AddDays(1);
                }
            }
            else if (dateRange == "Month")
            {
                while (compareDate < projectSettings.StartDate)
                {
                    var monthIndex = compareDate.Month;
                    var month = DateTimeFormatInfo.InvariantInfo.GetAbbreviatedMonthName(monthIndex);

                    labels.Add(month);

                    var registrations = registeredUsers.Count(u => u.ConfirmedUtc.Value.Year == compareDate.Year &&
                                                                   u.ConfirmedUtc.Value.Month == compareDate.Month);

                    chartData.Add(registrations);

                    compareDate = compareDate.AddMonths(1);
                }
            }

            var chartModel = new ChartLineViewModel
            {
                Header = header,
                Labels = labels,
                DataSetsLabel = labelDataSets,
                ChartData = chartData
            };

            return View(chartModel);
        }

    }
}

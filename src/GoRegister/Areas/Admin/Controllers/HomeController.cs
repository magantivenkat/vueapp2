using GoRegister.Areas.Admin.ViewModels;
using GoRegister.ApplicationCore.Domain.Registration.Services;
using GoRegister.ApplicationCore.Domain.Settings.Models;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Domain.Projects.Services;
using System.Security.Claims;

namespace GoRegister.Areas.Admin.Controllers
{
    public class HomeController : ProjectAdminControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ISettingsService _settingsService;
        private readonly IDelegateService _delegateService;

        public HomeController(IProjectService projectService, ISettingsService settingsService,
            IDelegateService delegateService)
        {
            _projectService = projectService;
            _settingsService = settingsService;
            _delegateService = delegateService;
        }

        [Route(ResetProjectRoute + "/")]
        public async Task<IActionResult> Index()
        {
            var project = await _settingsService.GetSettingsAsync<GeneralSettingsModel>();
            var delegates = await _delegateService.GetList();
            // log visit to recentProjects
            if (project.ProjectType == ApplicationCore.Data.Enums.ProjectTypeEnum.Project)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _projectService.AddProjectToRecentList(project.Id, userId);
            }

            var viewModel = new EventDashboardViewModel
            {
                IsLive = project.StatusId == ApplicationCore.Domain.Projects.Enums.ProjectStatus.Live,
                DaysToEvent = project.DaysToEvent,
                TotalRegistrations = delegates.Count(d => d.ConfirmedDate > d.InvitedDate)
            };

            return View(viewModel);
        }

        [Route(ResetProjectRoute + "/app/{*path}")]
        public async Task<IActionResult> App()
        {
            return View();
        }
    }
}
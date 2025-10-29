using GoRegister.Areas.Admin.ViewModels;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Models;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Services;
using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.ApplicationCore.Framework.Notifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Amazon.Runtime;
using Amazon.S3;
using System.IO;
using Amazon.S3.Transfer;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using MediatR;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Queries;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Commands;
using GoRegister.Filters;

namespace GoRegister.Areas.Admin.Controllers
{
    public class ThemesController : ProjectAdminControllerBase
    {
        private readonly INotifier _notifier;
        private readonly IProjectThemeService _projectThemeService;
        private readonly IConfiguration _configuration;
        private readonly IProjectSettingsAccessor _projectSettings;
        private readonly IMediator _mediator;

        public ThemesController(INotifier notifier, IProjectThemeService projectThemeService, IConfiguration configuration, IProjectSettingsAccessor projectSettings, IMediator mediator)
        {
            _notifier = notifier;
            _projectThemeService = projectThemeService;
            _configuration = configuration;
            _projectSettings = projectSettings;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _mediator.Send(new ProjectThemeEditQuery());
            return View(model);
        }

        [HttpPost, ModelStateValidation]
        public async Task<IActionResult> SaveTheme(ProjectThemeEditCommand model)
        {
            var result = await _mediator.Send(model);
            if(!result)
            {
                _notifier.Error("Something went wrong while saving your theme, please try again");
                return View(nameof(Index), model);
            }

            _notifier.Success("Theme saved successfully");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<int> PostTheme([FromForm] ProjectThemeModel model, IFormFile file)
        {
            if (file != null)
            {
                //Call logo upload function
                Guid logoGuid = Guid.NewGuid();
                model.ThemeUniqueId = logoGuid;

                model.File = file;

                //var result = await _projectThemeService.ProjectThemeUpload(model);

                //model.LogoUrl = Convert.ToString(result);
            }

            var newThemeId = _projectThemeService.SaveProjectThemeAsync(model);

            return newThemeId.Result;
        }

        

        [HttpGet]
        public async Task<IActionResult> GetTheme(int Id)
        {
            var model = await _projectThemeService.GetTheme(Id);

            model.LayoutOptions = _projectThemeService.GetLayoutOptions();

            return Json(model);
        }

        public IActionResult ThemeHistory(int id)
        {
            var list = _projectThemeService.GetProjectThemeByProjectIdAll(id);

            return View(list);
        }

        [HttpGet]
        public IActionResult EditThemeId(int id)
        {
            var model = _projectThemeService.GetProjectThemeById(id);

            return View("Index", model);
        }

    }
}

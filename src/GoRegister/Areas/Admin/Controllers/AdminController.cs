using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using GoRegister.ApplicationCore.Domain.Clients;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Models;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Services;
using GoRegister.Framework.Authorization;
using Microsoft.AspNetCore.Authorization;
using GoRegister.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[action]")]
    public class AdminController : AdminControllerBase
    {
        private readonly IProjectThemeService _projectThemeService;
        private readonly IClientService _clientService;
        private readonly IConfiguration _configuration;

        //Commented by Mandar Khade
        public AdminController(IProjectThemeService projectThemeService, IClientService clientService, IConfiguration configuration)
        {
            _projectThemeService = projectThemeService;
            _clientService = clientService;
            _configuration = configuration;
        }

        //Commented by Mandar Khade change 2

        private const string ThemeEditorView = "CreateEditTheme";

        [Route("~/Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policies.ManageGlobalThemes)]
        public async Task<IActionResult> Themes()
        {
            var list = await _projectThemeService.GetList();

            return View(list);
        }

        [Authorize(Policies.ManageGlobalThemes)]
        public async Task<IActionResult> CreateEditTheme([FromForm] ProjectThemeModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(ThemeEditorView);
            }

            var existingThemes = await _projectThemeService.GetList();
            var editTheme = existingThemes.Count(e => e.Name == model.Name);

            model.LayoutOptions = _projectThemeService.GetLayoutOptions();
            model.ClientList = await _clientService.GetClientDropdownList();

            if (model.Id == 0 && editTheme == 0 && model.Name != null)
            {
                Guid themeGuid = Guid.NewGuid();
                //Create new theme
                model.ThemeGuid = themeGuid;

                if (model.File != null)
                {
                    //Call logo upload function
                    Guid logoGuid = Guid.NewGuid();
                    model.ThemeUniqueId = logoGuid;

                    //var result = await _projectThemeService.ProjectThemeUpload(model);

                    //model.LogoUrl = Convert.ToString(result);
                }
                var newModel = await _projectThemeService.SaveGlobalProjectThemeAsync(model);

                var newModelEdit = _projectThemeService.GetProjectThemeByGuid(newModel.ThemeGuid);
                newModelEdit.LayoutOptions = _projectThemeService.GetLayoutOptions();
                return RedirectToAction("Themes", "Admin");

            }
            else if (model.Id != 0 && editTheme >= 1)
            {
                return View(model);
            }

            return View(model);
        }

        [Authorize(Policies.ManageGlobalThemes)]
        public IActionResult ThemePreviousVersions(Guid id)
        {
            var list = _projectThemeService.GetProjectThemeByGuidAll(id);

            return View(list);
        }

        [Authorize(Policies.ManageGlobalThemes)]
        public IActionResult UnarchiveTheme(Guid id)
        {
            _projectThemeService.UnarchiveGlobalTheme(id);

            return RedirectToAction("Themes");
        }

        [Authorize(Policies.ManageGlobalThemes)]
        public IActionResult ThemeArchived()
        {
            var list = _projectThemeService.GetArchivedGlobalThemes();

            return View(list);
        }


        [HttpGet]
        [Authorize(Policies.ManageGlobalThemes)]
        public async Task<IActionResult> EditThemeId(int id)
        {
            var model = _projectThemeService.GetProjectThemeById(id);
            model.ClientList = await _clientService.GetClientDropdownList();
            model.LayoutOptions = _projectThemeService.GetLayoutOptions();
            return View(ThemeEditorView, model);
        }

        [HttpPost]
        [Authorize(Policies.ManageGlobalThemes)]
        public async Task<IActionResult> PostTheme([FromForm] ProjectThemeModel model, IFormFile file)
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

            await _projectThemeService.SaveGlobalProjectThemeAsync(model);

            return Json(Url.Action("Themes", "Admin"));
        }

        //ThemeId
        [Authorize(Policies.ManageGlobalThemes)]
        public IActionResult PreviewTheme(int id)
        {

            var model = _projectThemeService.GetProjectThemeById(id);

            ViewBag.IsPreview = "Preview";
            ViewBag.themeId = model.Id;

            return View(model);
        }

        [HttpGet]
        [Authorize(Policies.ManageGlobalThemes)]
        public IActionResult GetClientThemes(int clientId)
        {
            var model = _projectThemeService.GetClientThemes(clientId)
                .Select(item => new SelectListItem() { Text = item.Name, Value = item.Id.ToString() }).ToList(); ;

            return new JsonResult(model);
        }      

    }
}
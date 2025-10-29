using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.CustomPages.Models;
using GoRegister.ApplicationCore.Domain.CustomPages.Services;
using GoRegister.ApplicationCore.Domain.ProjectPages.Models;
using GoRegister.ApplicationCore.Domain.ProjectPages.Services;
using GoRegister.Filters;
using GoRegister.ApplicationCore.Framework.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.Areas.Admin.Models;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using GoRegister.Areas.Admin.ViewModels;
using GoRegister.ApplicationCore.Domain.Settings.Services;

namespace GoRegister.Areas.Admin.Controllers
{
    public class PagesController : ProjectAdminControllerBase
    {
        private readonly IProjectPageService _projectPageService;


        private readonly ICustomPageService _customPageService;

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;        

        public PagesController(IProjectPageService projectPageService,
            ICustomPageService customPageService,
            ApplicationDbContext context, IConfiguration configuration)
        {
            _projectPageService = projectPageService;
            _customPageService = customPageService;
            _context = context;
            _configuration = configuration;
        }

        private const string EditorView = "CreateEdit";

        public async Task<IActionResult> Index()
        {
            var list = await _projectPageService.GetList();

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _customPageService.GetCreateEditModelAsync();

            return View(EditorView, model);
        }

        /// <summary>
        /// If create then this method creates a new Custom Page.
        /// If edit it saves the new values of the Custom Page and it also creates an old version of the previous values. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateEdit(CustomPageCreateEditModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _customPageService.GetCreateEditModelAsync(model);
                return View(EditorView, model);
            }

            Result<int> result;
            if (model.Id == 0)
            {
                var pages = _projectPageService.GetList().Result.Count();

                var projectPageModel = new ProjectPageModel
                {
                    Title = model.Title,
                    IsVisible = model.IsVisible,
                    Type = ProjectPage.PageType.Custom,
                    MenuPosition = pages + 1
                };
                var projectPageId = await _projectPageService.AddAsync(projectPageModel);

                model.ProjectPageId = projectPageId.Value;

                result = await _customPageService.CreateAsync(model);
                model.Id = result.Value;
            }
            else
            {
                var oldCustomPageVersion = await _customPageService.GetCurrentVersionAsync(model.Id);

                if (oldCustomPageVersion.Content != model.Content)
                {
                    await _customPageService.CreateVersionAsync(oldCustomPageVersion);
                }

                if (oldCustomPageVersion.IsVisible != model.IsVisible)
                {
                    await _customPageService.CreateAuditAsync(model);
                }

                result = await _customPageService.EditAsync(model);
                model.Id = result.Value;
            }


            if (result.Failed)
            {
                return View(EditorView);
            }

            return RedirectToAction("Edit", new { id = result.Value });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _customPageService.FindAsync(id);

            return View(EditorView, model);
        }

        [HttpGet]
        public async Task<IActionResult> EditRegistrationPage(int id)
        {
            var model = await _context.RegistrationPages.FindAsync(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRegistrationPage(ProjectPageModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _projectPageService.EditRegistrationPageAsync(model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _customPageService.FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelStateValidation]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _customPageService.FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            await _customPageService.DeleteAsync(model.Id);

            var projectPageMenuPosition = _projectPageService.FindAsync(model.ProjectPageId).Result.MenuPosition;

            await _projectPageService.DeleteAsync(model.ProjectPageId);

            var pageList = await _projectPageService.GetList();

            pageList = pageList.Where(c => c.MenuPosition > projectPageMenuPosition).ToList();

            foreach (var page in pageList)
            {
                page.MenuPosition -= 1;

                await _projectPageService.EditAsync(page);
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Invokes the InvokeAsync method of CustomPageVersionsViewComponent
        /// </summary>
        /// <param name="id">Id for CustomPageVersion</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CustomPageVersions(int id)
        {
            return ViewComponent("CustomPageVersions", id);
        }

        /// <summary>
        /// Invokes the InvokeAsync method of CustomPageAuditsViewComponent
        /// </summary>
        /// <param name="id">Id for CustomPageAudits</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CustomPageAudits(int id)
        {
            return ViewComponent("CustomPageAudits", id);
        }

        [HttpGet]
        public async Task<IActionResult> PreviewPage(int id)
        {
            var model = await _customPageService.FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        public async Task<IActionResult> PreviewVersionPage(int id)
        {
            var model = await _customPageService.GetCustomPageVersionAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RevertToPreviousVersion(int id)
        {
            // Populate CustomPageVersionModel
            var customPageVersionModel = await _customPageService.GetCustomPageVersionAsync(id);
            var customPageId = customPageVersionModel.CustomPageId;

            await SaveCurrentVersion(customPageId);

            // Populate CustomPageCreateEditModel and change its Content to the version you want to revert to
            var customPageCreateEditModel = await _customPageService.FindAsync(customPageId);
            customPageCreateEditModel.Content = customPageVersionModel.Content;
            var result = await _customPageService.EditAsync(customPageCreateEditModel);

            return RedirectToAction(nameof(Index));
        }


        private async Task SaveCurrentVersion(int customPageId)
        {
            // Get current version of the Custom Page and save a version of it
            var currentVersionAsync = await _customPageService.GetCurrentVersionAsync(customPageId);
            await _customPageService.CreateVersionAsync(currentVersionAsync);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateOrder(Dictionary<string, string> positions)
        {
            if (positions == null) return NoContent();

            foreach (var page in positions)
            {
                var pageId = page.Key;
                var menuPosition = page.Value;

                var isValidPageId = int.TryParse(pageId, out var id);
                var isValidPagePosition = int.TryParse(menuPosition, out var position);

                if (isValidPageId && isValidPagePosition)
                {
                    var model = await _projectPageService.FindAsync(id);
                    model.MenuPosition = position;

                    await _projectPageService.EditAsync(model);
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<JsonResult> CkEditorFileUpload(IFormFile upload)
        {
            var result = await _projectPageService.CkEditorFileUpload(upload);
           
            return Json(new { url = result });
        }

             
    }
}
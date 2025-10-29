using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoRegister.ApplicationCore.Domain.ProjectThemes.Services;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace GoRegister.Controllers
{
    public class ThemeController : Controller
    {
        private readonly IProjectThemeService _projectThemeService;
        private readonly IProjectSettingsAccessor _projectSettingsAccessor;

        public ThemeController (IProjectThemeService projectThemeService, IProjectSettingsAccessor projectSettingsAccessor)
        {
            _projectThemeService = projectThemeService;
            _projectSettingsAccessor = projectSettingsAccessor;
        }


        public async Task<IActionResult> OverrideCss(int id)
        {
            var projectTheme = await _projectThemeService.GetProjectThemeByProjectIdAsync(id);

            var overrideCss = "";

            if (projectTheme != null)
            {
                overrideCss = projectTheme.OverrideCss;
            }

            return Content(overrideCss, "text/css");
        }

        public async Task<IActionResult> ThemeCss(int id)
        {
            var projectTheme = await _projectThemeService.GetProjectThemeByProjectIdAsync(id);

            var themeCss = "";

            if (projectTheme != null)
            {
                themeCss = projectTheme.ThemeCss;
            }

            return Content(themeCss, "text/css");
        }

        public async Task<IActionResult> ThemeVariables(int id)
        {
            var projectTheme = await _projectThemeService.GetProjectThemeByProjectIdAsync(id);

            var themeVariables = "";

            if (projectTheme != null)
            {
                themeVariables = ":root {" + projectTheme.ThemeVariables + "}";
            }

            return Content(themeVariables, "text/css");
        }

        public async Task<IActionResult> PreviewOverrideCss(int id)
        {
            var projectTheme = _projectThemeService.GetProjectThemeById(id);

            return Content(projectTheme.OverrideCss, "text/css");
        }

        public async Task<IActionResult> PreviewThemeCss(int id)
        {
            var projectTheme =  _projectThemeService.GetProjectThemeById(id);

            return Content(projectTheme.ThemeCss, "text/css");
        }

        public async Task<IActionResult> PreviewThemeVariables(int id)
        {
            var projectTheme =  _projectThemeService.GetProjectThemeById(id);

            var themeVariables = ":root {" + projectTheme.ThemeVariables + "}";

            return Content(themeVariables, "text/css");
        }

        public async Task<IActionResult> HeaderScripts(int id)
        {
            var projectTheme = await _projectSettingsAccessor.GetTheme();

            var headerScripts = "";

            if (projectTheme != null)
            {
                headerScripts = projectTheme.HeadScripts;
            }

            return Content(headerScripts, "text/javascript");
        }

        public async Task<IActionResult> FooterScripts(int id)
        {
            var projectTheme = await _projectSettingsAccessor.GetTheme();

            var footerScripts = "";

            if (projectTheme != null)
            {
                footerScripts = projectTheme.FooterScripts;
            }

            //return Content(footerScripts, "text/javascript");
            return Content(HtmlEncoder.Default.Encode(footerScripts), "text/javascript"); //GOR-371
        }
    }
}
using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Framework;
using GoRegister.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Filters
{
    public class SitewidePasswordFilter : IAsyncActionFilter
    {
        private readonly ProjectTenant _project;
        private readonly IProjectSettingsAccessor _settingsAccessor;

        public SitewidePasswordFilter(ProjectTenant project, IProjectSettingsAccessor settingsAccessor)
        {
            _project = project;
            _settingsAccessor = settingsAccessor;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var settings = await _settingsAccessor.GetAsync();
            if (!_project.IsAdmin && settings.IsSitewidePasswordEnabled)
            {
                var request = context.HttpContext.Request;
                if(request.Path != Constants.SitePasswordUrl)
                {
                    var cookie = request.Cookies[Constants.SitePasswordCookie];
                    if (cookie != settings.SitewidePasswordHashed)
                    {
                        context.Result = new RedirectToActionResult("Index", "SitePassword", null);
                        return;
                    }
                }
            }

            await next();
        }
    }
}

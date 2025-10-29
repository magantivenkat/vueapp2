using GoRegister.ApplicationCore.Domain.Projects.Enums;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace GoRegister.Filters
{
    public class SiteAccessFilter : IAsyncAuthorizationFilter
    {
        private readonly ProjectTenant _project;
        private readonly IProjectSettingsAccessor _settingsAccessor;

        public SiteAccessFilter(ProjectTenant project, IProjectSettingsAccessor settingsAccessor)
        {
            _project = project;
            _settingsAccessor = settingsAccessor;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // if the current tenant is the admin skip this filter
            if (_project.IsAdmin) return;

            // if the endpoint allows anonymous users we'll skip this check
            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null) return;

            var user = context.HttpContext.User;

            var settings = await _settingsAccessor.GetAsync();
            if (settings.StatusId != ProjectStatus.Live)
            {
                // testing users can still view a site that IS NOT live               
                if (user.Identity.IsAuthenticated && user.HasClaim("IsTestDelegate", true.ToString()))
                {

                }
                else
                {
                    var notLiveResult = new ViewResult()
                    {
                        ViewName = "SiteNotLive",
                        StatusCode = 404
                    };
                    notLiveResult.StatusCode = 404;
                    context.Result = notLiveResult;
                }
            }
            else//Site is live
            {               
                if (!user.Identity.IsAuthenticated && !settings.AllowAnonymousAccess)
                {
                    context.Result = new RedirectToActionResult("Index", "User", new { returnUrl = context.HttpContext.Request.Headers["Referer"].ToString() });
                    return;
                }
                else
                {

                }
            }
        }
    }
}

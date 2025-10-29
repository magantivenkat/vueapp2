using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.ApplicationCore.Framework.Multitenancy;
using GoRegister.ApplicationCore.Framework.Multitenancy.Filters;
using GoRegister.Framework.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace GoRegister.Areas.Admin.Controllers
{
    [Area("Admin")]
    [TenantAdminFilter]
    [Authorize]
    public abstract class AdminControllerBase : Controller
    {

    }

    [Route(FullRoute), Authorize(Policy = Policies.AccessProjectAdmin)]
    public abstract class ProjectAdminControllerBase : AdminControllerBase
    {
        public const string ResetProjectRoute = "~/" + ProjectRoute;
        public const string ProjectRoute = "project/{projectId}";
        public const string IndexRoute = ResetProjectRoute + "/[controller]";
        public const string FullRoute = ProjectRoute + "/[controller]/[action]/{id?}";

        [NonAction]
        public override RedirectToActionResult RedirectToAction(
                string actionName,
                string controllerName,
                object routeValues,
                string fragment)
        {
            var rvd = routeValues != null ?
                new RouteValueDictionary(routeValues) :
                new RouteValueDictionary();

            var project = HttpContext?.GetTenant();
            if (project != null)
            {
                rvd.Add("projectId", project.Id);
            }

            return base.RedirectToAction(actionName, controllerName, rvd, fragment);
        }
    }

    [Area("Admin")]
    [TenantAdminFilter]
    [Authorize]
    [ApiController]
    public abstract class ApiAdminControllerBase : ControllerBase
    {
        private IMediator _mediatorOverride;
        public IMediator Mediator
        {
            get
            {
                return _mediatorOverride ?? HttpContext?.RequestServices.GetRequiredService<IMediator>();
            }
            set { _mediatorOverride = value; }
        }

        private IProjectSettingsAccessor _projectSettingsAccessorOverride;
        public IProjectSettingsAccessor ProjectSettingsAccessor
        {
            get
            {
                return _projectSettingsAccessorOverride ?? HttpContext?.RequestServices.GetRequiredService<IProjectSettingsAccessor>();
            }
            set { _projectSettingsAccessorOverride = value; }
        }

        public ApiAdminControllerBase()
        {
        }
    }

    [Route(FullRoute)]
    public abstract class ApiProjectAdminControllerBase : ApiAdminControllerBase
    {
        public const string ResetProjectRoute = "~/" + ProjectRoute;
        public const string ProjectRoute = "project/{projectId}/api";
        public const string IndexRoute = ResetProjectRoute + "/[controller]";
        public const string FullRoute = ProjectRoute + "/[controller]/[action]/{id?}";
    }
}

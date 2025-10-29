/*MRF Changes: Change redirect page name to MRF request
Modified Date: 22nd September 2022
Modified By: Mandar.Khade@amexgbt.com
Team member: Harish.Rame@amexgbt.com
JIRA Ticket No: GoRegister / GOR - 221
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace GoRegister.ApplicationCore.Framework.Multitenancy.Filters
{
    public class TenantAdminFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var _projectTenant = context.HttpContext.RequestServices.GetService<ProjectTenant>();
            if (!_projectTenant.IsAdmin)
            {
                context.Result = new RedirectResult("~/");
            }
        }
    }

    public class TenantFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var _projectTenant = context.HttpContext.RequestServices.GetService<ProjectTenant>();
            if(_projectTenant.IsAdmin)
            {
                // context.Result = new RedirectResult("~/projects");

                context.Result = new RedirectResult("~/requests");
            }
        }
    }

    public class TenantFilter : IActionFilter
    {
        private readonly ProjectTenant _project;

        public TenantFilter(ProjectTenant project)
        {
            _project = project;
        }
        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var area = (string)context.RouteData.Values["area"] ?? string.Empty;
            if (_project.IsAdmin && (area != "Admin" && area != "Identity"))
            {
               // context.Result = new RedirectResult("~/projects");

                context.Result = new RedirectResult("~/requests");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Areas.Admin.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string ProjectAction(this IUrlHelper url, string action)
        {
            return url.Action(action, new
            {
                projectId = url.ActionContext.RouteData.Values["projectId"]
            });
        }

        public static string ProjectAction(this IUrlHelper url, string action, object routeValues)
        {
            var routeValueDictionary = AddProjectIdToRouteValues(url, routeValues);
            return url.Action(action, routeValueDictionary);
        }

        public static string ProjectAction(this IUrlHelper url, string action, string controller, object routeValues)
        {
            var routeValueDictionary = AddProjectIdToRouteValues(url, routeValues);
            return url.Action(action, controller, routeValueDictionary);
        }

        private static RouteValueDictionary AddProjectIdToRouteValues(IUrlHelper url, object routeValues)
        {
            var projectId = url.ActionContext.RouteData.Values["projectId"];
            var routeValueDictionary = new RouteValueDictionary(routeValues);
            routeValueDictionary["projectId"] = projectId;
            return routeValueDictionary;
        }
    }
}

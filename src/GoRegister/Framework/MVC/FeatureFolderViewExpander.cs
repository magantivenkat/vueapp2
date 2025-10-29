using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Framework.MVC
{
    public class FeatureViewExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var controllerActionDescriptor = context.ActionContext.ActionDescriptor as ControllerActionDescriptor;
            string featureName = controllerActionDescriptor?.Properties["feature"] as string;
            foreach (var location in viewLocations)
            {
                yield return location.Replace("{3}", featureName);
            }
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["displayname"] = context.ActionContext.ActionDescriptor.DisplayName;
        }
    }

    public static class RazorViewEngineOptionsExtensions
    {
        public static void AddFeatureViewLocation(this RazorViewEngineOptions options)
        {
            options.ViewLocationFormats.Insert(0, "/Features/Shared/{0}.cshtml");
            options.ViewLocationFormats.Insert(0, "/Features/{3}/{0}.cshtml");
            options.ViewLocationFormats.Insert(0, "/Features/{3}/{1}/{0}.cshtml");
            options.AreaViewLocationFormats.Insert(0, "/Areas/{2}/Features/Shared/{0}.cshtml");
            options.AreaViewLocationFormats.Insert(0, "/Areas/{2}/Features/{3}/{0}.cshtml");
            options.AreaViewLocationFormats.Insert(0, "/Areas/{2}/Features/{3}/{1}/{0}.cshtml");
        }
    }
}

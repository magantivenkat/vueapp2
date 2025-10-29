using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using GoRegister.ApplicationCore.Framework.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Framework.Multitenancy
{
    public static class FrameworkRegistration
    {
        public static IServiceCollection AddGoRegister(this IServiceCollection services)
        {
            services.AddNotifications();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x => {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                if (actionContext == null) return null;

                return factory.GetUrlHelper(actionContext);
            });

            //services.Configure<RazorViewEngineOptions>(options => {
            //    // add Themes view location
            //    options.ViewLocationExpanders.Add(new ThemeViewExpander());
            //});

            return services;
        }
    }
}

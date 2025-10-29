using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace GoRegister.ApplicationCore.Framework.Multitenancy
{
    public static class MultitenancyServiceCollectionExtensions
    {
        public static IServiceCollection AddMultitenancy<TTenant, TResolver>(this IServiceCollection services)
            where TResolver : class, ITenantResolver
            where TTenant : class
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));

            services.AddScoped<ITenantResolver, TResolver>();

            // No longer registered by default as of ASP.NET Core RC2
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<ISingletonTenantAccessor, SingletonTenantAccessor>();

            // Make Tenant and TenantContext injectable
            //services.AddScoped(prov => prov.GetService<IHttpContextAccessor>()?.HttpContext?.GetTenantContext());
            //services.AddScoped(prov => prov.GetService<TenantContext>()?.Tenant);
            services.AddScoped<ITenantAccessor, TenantAccessor>();

            services.TryAddScoped<ProjectTenant>(prov => prov.GetRequiredService<ITenantAccessor>().Get);


            return services;
        }

        public static IServiceCollection WithPerTenantOptions<TOptions>(this IServiceCollection services, Action<TOptions, ProjectTenant> tenantInfo) where TOptions : class, new()
        {
            if (tenantInfo == null)
            {
                throw new ArgumentNullException(nameof(tenantInfo));
            }

            // Handles multiplexing cached options.
            services.TryAddSingleton<IOptionsMonitorCache<TOptions>>(sp =>
            {
                return (MultiTenantOptionsCache<TOptions>)
                    ActivatorUtilities.CreateInstance(sp, typeof(MultiTenantOptionsCache<TOptions>));
            });

            // Necessary to apply tenant options in between configuration and postconfiguration
            services.TryAddTransient<IOptionsFactory<TOptions>>(sp =>
            {
                return (IOptionsFactory<TOptions>)ActivatorUtilities.
                    CreateInstance(sp, typeof(MultiTenantOptionsFactory<TOptions>), new[] { tenantInfo });
            });

            services.TryAddScoped<IOptionsSnapshot<TOptions>>(sp => BuildOptionsManager<TOptions>(sp));

            services.TryAddSingleton<IOptions<TOptions>>(sp => BuildOptionsManager<TOptions>(sp));

            return services;
        }

        private static MultiTenantOptionsManager<TOptions> BuildOptionsManager<TOptions>(IServiceProvider sp) where TOptions : class, new()
        {
            var cache = ActivatorUtilities.CreateInstance(sp, typeof(MultiTenantOptionsCache<TOptions>));
            return (MultiTenantOptionsManager<TOptions>)
                ActivatorUtilities.CreateInstance(sp, typeof(MultiTenantOptionsManager<TOptions>), new[] { cache });
        }
    }
}

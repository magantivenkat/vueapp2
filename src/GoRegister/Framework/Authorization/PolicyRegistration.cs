using GoRegister.ApplicationCore.Framework.Identity;
using GoRegister.Framework.Authorization.Handlers;
using GoRegister.Framework.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace GoRegister.Framework.Authorization
{
    public static class PolicyRegistration
    {
        public static void AddPolicies(this AuthorizationOptions options)
        {
            options.AddPolicy(Policies.ManageGlobalThemes, p => p.RequireRole(Roles.DigitalServices));

            options.AddPolicy(Policies.ManageUsers, p => p.RequireRole(Roles.Administrator));

            options.AddPolicy(Policies.ManageClients, p => p.RequireRole(Roles.DigitalServices));

            options.AddPolicy(Policies.ManageDomains, p => p.RequireRole(Roles.Administrator));

            options.AddPolicy(Policies.AccessProjectAdmin, p =>
            {
                p.RequireAuthenticatedUser();
                p.Requirements.Add(new AccessProjectAdminRequirement());
            });
        }

        public static IServiceCollection AddAuthorizationHandlers(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, HasAccessAllProjectsRoleHandler>();
            services.AddTransient<IAuthorizationHandler, MappedToProjectHandler>();

            return services;
        }
    }
}

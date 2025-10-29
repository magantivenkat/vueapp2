using Microsoft.AspNetCore.Builder;
using GoRegister.ApplicationCore.Framework;
using GoRegister.ApplicationCore.Framework.Multitenancy.Internal;
using System;
using Microsoft.Extensions.Configuration;

namespace GoRegister.ApplicationCore.Framework.Multitenancy
{
    public static class MultitenancyApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMultitenancy<TTenant>(this IApplicationBuilder app, IConfiguration configuration)
        {
            _ = app ?? throw new ArgumentNullException(nameof(app));
            app.UseMiddleware<TenantResolutionMiddleware>();
            return app.UseMiddleware<TenantUnresolvedMiddleware>(configuration.GetValue<string>("TenantNotFoundRedirectLocation"));
        }
    }
}

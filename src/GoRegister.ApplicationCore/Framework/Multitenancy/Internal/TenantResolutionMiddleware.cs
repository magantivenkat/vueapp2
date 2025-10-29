using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Serilog.Context;

namespace GoRegister.ApplicationCore.Framework.Multitenancy.Internal
{
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public TenantResolutionMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _ = next ?? throw new ArgumentNullException(nameof(next));
            _ = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

            _next = next;
            _logger = loggerFactory.CreateLogger<TenantResolutionMiddleware>();
        }

        public async Task Invoke(HttpContext context, ITenantResolver tenantResolver)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));
            _ = tenantResolver ?? throw new ArgumentNullException(nameof(tenantResolver));

            _logger.LogDebug("Resolving TenantContext using {loggerType}.", tenantResolver.GetType().Name);

            // get the current tenant
            var tenant = await tenantResolver.ResolveAsync(context);
            // if we didn't find a tenant continue with the default pipeline
            if (tenant == null)
            {
                _logger.LogDebug("Tenant could not be resolved");
                await _next(context);
                return;
            }

            // for tenants that use a url like foo.com/bar we need to set the pathbase and reset the path so we route resources correctly
            // I'm not sure if this is the correct way to handle this. It means we do not have to prefix all our routes with {tenant}
            // and I was not able to make that route optional for tenants where this is prefix, but just a host
            if (!string.IsNullOrEmpty(tenant.Prefix))
            {
                PathString prefix = "/" + tenant.Prefix;
                context.Request.PathBase += prefix;
                context.Request.Path.StartsWithSegments(prefix, StringComparison.OrdinalIgnoreCase, out PathString remainingPath);
                context.Request.Path = remainingPath;
            }

            _logger.LogDebug("Tenant resolved. Adding to HttpContext");
            context.SetTenantContext(tenant);

            // add tenant properties to serilog context
            using (LogContext.PushProperty("TenantId", tenant.Id))
            using (LogContext.PushProperty("IsTenantAdmin", tenant.IsProjectAdmin || tenant.IsAdmin))
            {
                await _next(context);
            }

        }
    }
}

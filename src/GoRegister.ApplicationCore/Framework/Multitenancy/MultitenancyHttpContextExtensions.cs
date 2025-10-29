using Microsoft.AspNetCore.Http;
using System;

namespace GoRegister.ApplicationCore.Framework.Multitenancy
{
	/// <summary>
    /// Multitenant extensions for <see cref="HttpContext"/>.
    /// </summary>
    public static class MultitenancyHttpContextExtensions
    {
        private const string TenantContextKey = "GoRegister.Framework.TenantContext";

        public static void SetTenantContext(this HttpContext context, ProjectTenant tenantContext)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));
            _ = tenantContext ?? throw new ArgumentNullException(nameof(tenantContext));

            context.Items[TenantContextKey] = tenantContext;
        }

        public static ProjectTenant GetTenant(this HttpContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));

            object tenantContext;
            if (context.Items.TryGetValue(TenantContextKey, out tenantContext))
            {
                return tenantContext as ProjectTenant;
            }

            return null;
        }
    }
}

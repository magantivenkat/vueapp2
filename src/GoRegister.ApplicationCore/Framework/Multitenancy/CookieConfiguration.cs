using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Framework.Multitenancy
{
    public class CookieConfiguration : IConfigureNamedOptions<CookieAuthenticationOptions>
    {
        private readonly ISingletonTenantAccessor _tenantContextAccessor;

        public CookieConfiguration(ISingletonTenantAccessor tenantContextAccessor)
        {
            _tenantContextAccessor = tenantContextAccessor;
        }

        public void Configure(string name, CookieAuthenticationOptions options)
        {
            var tenant = _tenantContextAccessor.Get;

            if (tenant == null)
            {
                options.Cookie.Name = "AspNet.Cookies";            }
            else
            {
                options.Cookie.Name = $"{tenant.Name}.AspNet.Cookies";
                if(!string.IsNullOrWhiteSpace(tenant.Prefix))
                {
                    options.Cookie.Path = $"/{tenant.Prefix}"; 
                }
            }
        }

        public void Configure(CookieAuthenticationOptions options)
            => Configure(Options.DefaultName, options);
    }
}

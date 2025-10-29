using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System;

namespace GoRegister.ApplicationCore.Framework.Multitenancy
{
    public class AuthOptionsFactory : IOptionsFactory<CookieAuthenticationOptions>
    {
        private readonly Action<CookieAuthenticationOptions, ProjectTenant> _tenantConfig;
        private readonly ISingletonTenantAccessor _tenantContextAccessor;

        public AuthOptionsFactory(Action<CookieAuthenticationOptions, ProjectTenant> tenantConfig, ISingletonTenantAccessor tenantContextAccessor)
        {
            _tenantContextAccessor = tenantContextAccessor;
            _tenantConfig = tenantConfig;
        }


        public CookieAuthenticationOptions Create(string name)
        {
            var options = new CookieAuthenticationOptions();
            var tenant = _tenantContextAccessor.Get;


            if (tenant != null)
            {
                _tenantConfig(options, tenant);
            }

            return options;
        }
    }
}

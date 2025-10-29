using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Framework.Multitenancy
{
    public class TenantContext
    {
        public TenantContext(ProjectTenant tenant)
        {
            _ = tenant ?? throw new ArgumentNullException(nameof(tenant));

            Tenant = tenant;
        }

        public string Id { get; } = Guid.NewGuid().ToString();
        public ProjectTenant Tenant { get; }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Framework.Multitenancy
{
    public interface ISingletonTenantAccessor
    {
        ProjectTenant Get { get; }
    }

    public class SingletonTenantAccessor : ISingletonTenantAccessor
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public SingletonTenantAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public ProjectTenant Get => httpContextAccessor.HttpContext?.GetTenant();
    }

    public interface ITenantAccessor
    {
        ProjectTenant Get { get; }
        void SetTenant(ProjectTenant tenant);
    }

    public class TenantAccessor : ITenantAccessor
    {
        private readonly ISingletonTenantAccessor _tenantAccessor;
        private ProjectTenant _local = null;

        public TenantAccessor(ISingletonTenantAccessor tenantAccessor)
        {
            _tenantAccessor = tenantAccessor;
        }

        public ProjectTenant Get => _local ?? _tenantAccessor.Get;

        public void SetTenant(ProjectTenant tenant)
        {
            _local = tenant;
        }
    }
}

using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Framework.Multitenancy
{
    public interface ITenantResolver
    {
        Task<ProjectTenant> ResolveAsync(HttpContext context);
    }
}

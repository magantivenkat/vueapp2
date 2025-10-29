using GoRegister.ApplicationCore.Framework.Identity;
using GoRegister.Framework.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Framework.Authorization.Handlers
{
    public class HasAccessAllProjectsRoleHandler : AuthorizationHandler<AccessProjectAdminRequirement>
    {
        private static readonly string[] _roles = { Roles.DigitalServices, Roles.Administrator };

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessProjectAdminRequirement requirement)
        {
            if (context.User != null)
            {
                if(_roles.Any(context.User.IsInRole))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}

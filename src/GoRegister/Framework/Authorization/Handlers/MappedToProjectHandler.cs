using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using GoRegister.ApplicationCore.Domain.Settings.Services;
using GoRegister.Framework.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Framework.Authorization.Handlers
{
    public class MappedToProjectHandler : AuthorizationHandler<AccessProjectAdminRequirement>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProjectSettingsAccessor _projectAccessor;
        private readonly ApplicationDbContext _db;

        public MappedToProjectHandler(UserManager<ApplicationUser> userManager, IProjectSettingsAccessor projectAccessor, ApplicationDbContext db)
        {
            _userManager = userManager;
            _projectAccessor = projectAccessor;
            _db = db;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       AccessProjectAdminRequirement requirement)
        {
            // skip this if we've already passed
            if (context.HasSucceeded) return;

            if (context.User != null && context.User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(_userManager.GetUserId(context.User));
                var project = await _projectAccessor.GetAsync();
                if (project == null) return;

                // check if user is project owner
                if (project.CreatedByUserId == userId)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    // check if user is mapped to this project
                    var mappedToProject = _db.UserProjectMappings.Any(e => e.UserId == userId && e.ProjectId == project.Id);
                    if(mappedToProject)
                    {
                        context.Succeed(requirement);
                    }
                }
            }

            return;
        }
    }
}

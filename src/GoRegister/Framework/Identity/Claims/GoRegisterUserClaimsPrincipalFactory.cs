using GoRegister.ApplicationCore.Data;
using GoRegister.ApplicationCore.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GoRegister.Framework.Identity.Claims
{
    public class GoRegisterUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, UserRole>
    {
        private readonly ApplicationDbContext _db;

        public GoRegisterUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<UserRole> roleManager, IOptions<IdentityOptions> optionsAccessor, ApplicationDbContext db)
            : base(userManager, roleManager, optionsAccessor)
        {
            _db = db;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // admin user
            if (user.ProjectId == null || user.ProjectId == 1)
            {

            }
            // delegate
            else
            {
                var delegateUser = _db.Delegates.First(e => e.Id == user.Id);

                // is delegate a test?
                identity.AddClaim(new Claim("IsTestDelegate", delegateUser.IsTest.ToString(), ClaimValueTypes.Boolean));
            }

            return identity;
        }
    }
}

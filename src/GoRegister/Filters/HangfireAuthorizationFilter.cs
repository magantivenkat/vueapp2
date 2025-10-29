using GoRegister.ApplicationCore.Framework.Identity;
using Hangfire.Dashboard;

namespace GoRegister.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var user = context.GetHttpContext().User;
            // Only allow administrators access
            return user.Identity.IsAuthenticated && user.IsInRole(Roles.Administrator);
        }
    }
}
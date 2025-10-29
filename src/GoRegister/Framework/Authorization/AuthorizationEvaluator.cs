using GoRegister.ApplicationCore.Framework.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GoRegister.Framework.Authorization
{
    public class AuthorizationEvaluator : IAuthorizationEvaluator
    {
        /// <summary>
        /// Determines whether the authorization result was successful or not.
        /// </summary>
        /// <param name="context">The authorization information.</param>
        /// <returns>The <see cref="AuthorizationResult"/>.</returns>
        public AuthorizationResult Evaluate(AuthorizationHandlerContext context)
            => context.HasSucceeded || context.User.IsInRole(Roles.Administrator)
                  ? AuthorizationResult.Success()
                  : AuthorizationResult.Failed(context.HasFailed
                      ? AuthorizationFailure.ExplicitFail()
                      : AuthorizationFailure.Failed(context.PendingRequirements));
    }
}

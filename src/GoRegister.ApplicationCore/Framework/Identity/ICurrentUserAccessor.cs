using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GoRegister.ApplicationCore.Framework.Identity
{
    public interface ICurrentUserAccessor
    {
        ClaimsPrincipal Get { get; }
        int? GetUserId();
    }

    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal Get => _httpContextAccessor.HttpContext.User;

        /// <summary>
        /// Gets the id of the current user
        /// </summary>
        /// <remarks>Null if the ClaimTypes.NameIdentifier does not exist</remarks>
        public int? GetUserId()
        {
            var userId = Get.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return null;
            return int.Parse(userId);
        }
    }
}

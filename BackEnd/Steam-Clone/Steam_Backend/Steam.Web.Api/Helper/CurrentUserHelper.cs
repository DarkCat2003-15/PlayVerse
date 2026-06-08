using SteamShared.Constants;
using System.Security.Claims;

namespace Steam.Web.Api.Helper
{
    public static class CurrentUserHelper
    {
        public static Guid GetRequiredUserId(ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimsConstants.USERS_ID);

            if (claim == null || !Guid.TryParse(claim.Value, out var userId))
            {
                throw new UnauthorizedAccessException(ResponseConstants.AUTH_CLAIM_USER_NOT_FOUND);
            }

            return userId;
        }

        public static bool HasRole(ClaimsPrincipal user, string roleName)
        {
            return user.Claims.Any(x => x.Type == ClaimsConstants.ROLE && x.Value == roleName);
        }

        public static bool HasPermission(ClaimsPrincipal user, string permission)
        {
            return user.Claims.Any(x => x.Type == ClaimsConstants.PERMISSION && x.Value == permission);
        }
    }
}

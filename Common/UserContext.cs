using System.Security.Claims;
using System.Linq;

namespace student_log_api.Common
{
    public static class UserContext
    {
        public static bool ValidateUser(
            ClaimsPrincipal user,
            out int loginUserID,
            out int loginAccountID,
            out string message)
        {
            loginUserID = 0;
            loginAccountID = 0;
            message = string.Empty;

            var userId = user.FindFirst("UserID")?.Value;

            if (string.IsNullOrEmpty(userId) ||
                !int.TryParse(userId, out loginUserID))
            {
                message = "UserID claim is missing or invalid.";
                return false;
            }

            var accountId = user.FindFirst("AccountID")?.Value;

            if (string.IsNullOrEmpty(accountId) ||
                !int.TryParse(accountId, out loginAccountID))
            {
                message = "AccountID claim is missing or invalid.";
                return false;
            }

            return true;
        }

        public static bool HasSchoolAccess(ClaimsPrincipal user, int schoolID)
        {
            var schoolIDs = user.FindFirst("SchoolIDs")?.Value;
            if (schoolID <= 0 || string.IsNullOrWhiteSpace(schoolIDs)) return false;
            return schoolIDs.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(value => int.TryParse(value.Trim(), out var id) ? id : 0)
                .Contains(schoolID);
        }
    }
}
using System.Security.Claims;

namespace Project.interfaces
{
    public interface IAuthorizationService
    {
        (string UserType, int UserId) GetUserClaims(ClaimsPrincipal user);
        bool IsAccessDenied(int id, string userType, int userId);

    }
}

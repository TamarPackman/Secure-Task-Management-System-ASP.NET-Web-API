using System.Security.Claims;
using Project.interfaces;
namespace Project.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public (string UserType, int UserId) GetUserClaims(ClaimsPrincipal user)
        {
            string type = user.FindFirst("Type")?.Value!;
            int userId = int.Parse(user.FindFirst("UserId")?.Value!);
            return (type, userId);
        }
        public bool IsAccessDenied(int id, string userType, int userId)
        {
            return userType == "User" && id != userId;
        }
    }
    public static partial class ServiceHelper
    {
        public static void AddAuthorizationService(this IServiceCollection BuilderServices)
        {
            BuilderServices.AddSingleton<IAuthorizationService, AuthorizationService>();
        }
    }
}

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Project.Models;

namespace Project.interfaces
{
    public interface ITokenService
    {
       SecurityToken GetToken(List<Claim> claims);
       TokenValidationParameters GetTokenValidationParameters();
      string  WriteToken(SecurityToken token);

    }
}
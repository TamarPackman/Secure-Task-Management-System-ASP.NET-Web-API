
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Project.interfaces;

namespace Project.Services;

public class TokenService:ITokenService
{
    private  SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));
    private  string issuer = "https://jewelry-demo.com";
    public  SecurityToken GetToken(List<Claim> claims) =>
            new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                expires: DateTime.Now.AddDays(30.0),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

    public  TokenValidationParameters GetTokenValidationParameters() =>
        new TokenValidationParameters
        {
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero // remove delay of token when expire
        };

    public  string WriteToken(SecurityToken token) =>
        new JwtSecurityTokenHandler().WriteToken(token);

//      public static  string  GetDetailFromToken(string token,string detail)
// {

//     token = token.Replace("Bearer ", "").Trim();
//     var handler = new JwtSecurityTokenHandler();
//     var jwtToken = handler.ReadJwtToken(token);
//     string userFromToken =jwtToken.Claims.FirstOrDefault(c => c.Type == detail)?.Value;
//     return userFromToken;
// }
}
public static partial class ServiceHelper
    {
        public static void AddTokenService(this IServiceCollection BuilderServices)
        {
            BuilderServices.AddSingleton<ITokenService, TokenService>();
        }
    }
    
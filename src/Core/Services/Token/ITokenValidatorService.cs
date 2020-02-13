using System.IdentityModel.Tokens.Jwt;

namespace CustomClaims.Core.Services.Token
{
    public interface ITokenValidatorService
    {
        JwtSecurityToken GetJwtSecurityToken(string token);
    }
}
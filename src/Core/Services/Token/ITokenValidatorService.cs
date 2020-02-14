using System.IdentityModel.Tokens.Jwt;

namespace Security.Core.Services.Token
{
    public interface ITokenValidatorService
    {
        JwtSecurityToken GetJwtSecurityToken(string token);
    }
}
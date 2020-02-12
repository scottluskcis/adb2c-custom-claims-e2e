using System.Threading;
using System.Threading.Tasks;
using CustomClaims.Core.Models;

namespace CustomClaims.Core.Services.Token
{
    public interface ITokenService
    {
        Task<TokenResponse> GetTokenAsync(
            string code, 
            CancellationToken cancellationToken = default);

        Task<TokenResponse> RefreshTokenAsync(
            string refreshToken, 
            CancellationToken cancellationToken = default);
    }
}
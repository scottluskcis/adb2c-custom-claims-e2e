using System.Threading;
using System.Threading.Tasks;
using Security.Core.Models;

namespace Security.Core.Services.Token
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
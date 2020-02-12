using System.Threading;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Services.Token
{
    public interface ITokenService
    {
        Task<TokenResponse> GetTokenAsync(
            string code, 
            CancellationToken cancellationToken = default);
    }
}
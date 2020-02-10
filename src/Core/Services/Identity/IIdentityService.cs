using Core.Models;
using System.Threading.Tasks;

namespace Core.Services.Identity
{
    public interface IIdentityService
    {
        Task<OutputClaimsModel> SignUpAsync(InputClaimsModel model);
        Task<OutputClaimsModel> SignInAsync(InputClaimsModel model);
    }
}

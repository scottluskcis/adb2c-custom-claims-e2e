using System.Threading.Tasks;
using Security.Core.Models;

namespace Security.Core.Services.Identity
{
    public interface IIdentityService
    {
        Task<OutputClaimsModel> SignUpAsync(InputClaimsModel model);
        Task<OutputClaimsModel> SignInAsync(InputClaimsModel model);
    }
}

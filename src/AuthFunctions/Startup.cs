using Core.Services.Identity;
using Core.Services.Token;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AuthFunctions.Startup))]
namespace AuthFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();

            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddTransient<IIdentityService, IdentityService>();
        }
    }
}

using AuthFunctions.Extensions;
using CustomClaims.Core.Options;
using CustomClaims.Core.Services.Identity;
using CustomClaims.Core.Services.Token;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AuthFunctions.Startup))]
namespace AuthFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddConfiguration();

            builder.Services.AddHttpClient();

            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddTransient<IIdentityService, IdentityService>();

            builder.Services.AddSingleton<IAzureAdb2COptions>((provider) =>
            {
                var config = provider.GetService<IConfiguration>();
                var prefix = "Adb2c";
                var options = new AzureAdb2COptions
                {
                    ClientId = config[$"{prefix}:ClientId"],
                    Tenant = config[$"{prefix}:Tenant"],
                    SignUpSignInPolicyId = config[$"{prefix}:SignUpSignInPolicyId"],
                    ResetPasswordPolicyId = config[$"{prefix}:ResetPasswordPolicyId"],
                    EditProfilePolicyId = config[$"{prefix}:EditProfilePolicyId"],
                    RedirectUri = config[$"{prefix}:RedirectUri"],
                    ClientSecret = config[$"{prefix}:ClientSecret"],
                    ApiUrl = config[$"{prefix}:ApiUrl"],
                    ApiScopes = config[$"{prefix}:ApiScopes"]
                };
                return options;
            });
        }

    }
}

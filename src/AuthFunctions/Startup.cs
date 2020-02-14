using AuthFunctions.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Security.Core.Options;
using Security.Core.Services.Identity;
using Security.Core.Services.Token;

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

            builder.Services.AddSingleton<ITokenValidationOptions>((provider) =>
            {
                var config = provider.GetService<IConfiguration>();
                var prefix = "Adb2c";
                var options = new TokenValidationOptions
                {
                    ClientId = config[$"{prefix}:ClientId"],
                    Issuer = config[$"{prefix}:Issuer"],
                    RsaModulus = config[$"{prefix}:RsaModulus"],
                    RsaExponent = config[$"{prefix}:RsaExponent"]
                };
                return options;
            });
        }

    }
}

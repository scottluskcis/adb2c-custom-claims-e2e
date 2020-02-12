using System;
using System.Linq;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Core.Options;
using Core.Services.Identity;
using Core.Services.Token;
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
            var config = builder.Services.GetConfiguration();

            builder.Services.AddHttpClient();

            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddTransient<IIdentityService, IdentityService>();

            builder.Services.AddOptions<AzureAdb2cOptions>().Configure(o =>
            {
                o.ClientId = "c998a7a7-bd4d-45fd-9d3c-befa5e1d7cd4";
                o.Tenant = "customaadb2c";
                o.SignUpSignInPolicyId = "B2C_1A_signup_signin";
                o.ResetPasswordPolicyId = "B2C_1A_PasswordReset";
                o.EditProfilePolicyId = "B2C_1A_ProfileEdit";
                o.RedirectUri = "https://jwt.ms";
                o.ClientSecret = "s]5{AZfS8I8p1/{UK!]56h;(";
                o.ApiUrl = "";
                o.ApiScopes = "openid offline_access https://customaadb2c.onmicrosoft.com/api/read";
            });

            var vaultUrl = config["VaultUrl"];
            if (!string.IsNullOrEmpty(vaultUrl))
            {
                builder.Services.AddSingleton((provider) => new SecretClient(new Uri(vaultUrl), new DefaultAzureCredential()));
            }
        }
    }

    public static class StartupExtensions
    {
        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            var configurationBuilder = new ConfigurationBuilder();
            
            var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IConfiguration));
            if (descriptor?.ImplementationInstance is IConfigurationRoot configuration)
            {
                configurationBuilder.AddConfiguration(configuration);
            }

            var config = configurationBuilder.Build();
            return config;
        }
    }
}

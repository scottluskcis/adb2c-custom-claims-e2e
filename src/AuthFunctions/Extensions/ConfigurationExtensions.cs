using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AuthFunctions.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration AddConfiguration(this IServiceCollection services)
        {
            var configBuilder = services.GetConfigurationBuilder()
                .SetBasePath(GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("local.settings.json", optional: true)
                .AddSecrets()
                .AddEnvironmentVariables();
            
            var config = configBuilder.Build();
            services.ReplaceConfiguration(config);

            return config;
        }

        public static IConfigurationBuilder AddSecrets(this IConfigurationBuilder configurationBuilder)
        {
            var keyVaultUri = Environment.GetEnvironmentVariable("KeyVaultUri");
            return !string.IsNullOrWhiteSpace(keyVaultUri) 
                ? configurationBuilder.AddAzureKeyVault(keyVaultUri) 
                : configurationBuilder;
        }

        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            var configurationBuilder = services.GetConfigurationBuilder();
            var config = configurationBuilder.Build();
            return config;
        }

        public static IConfigurationBuilder GetConfigurationBuilder(this IServiceCollection services)
        {
            var configurationBuilder = new ConfigurationBuilder();
            
            var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IConfiguration));
            if (descriptor?.ImplementationInstance is IConfigurationRoot configuration)
            {
                configurationBuilder.AddConfiguration(configuration);
            }
            
            return configurationBuilder;
        }

        public static void ReplaceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Replace(ServiceDescriptor.Singleton(typeof(IConfiguration), configuration));
        }

        public static string GetCurrentDirectory()
        {
            var currentDirectory = "/home/site/wwwroot";
            var isLocal = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"));
            if (isLocal)
            {
                currentDirectory = Environment.CurrentDirectory;
            }
            return currentDirectory;
        }
    }
}
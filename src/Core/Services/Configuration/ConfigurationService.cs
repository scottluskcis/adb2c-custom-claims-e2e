using Microsoft.Extensions.Configuration;

namespace CustomClaims.Core.Services.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


    }

    public interface IConfigurationService
    {
    }
}

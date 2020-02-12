using Microsoft.Extensions.Configuration;

namespace Core.Services.Configuration
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

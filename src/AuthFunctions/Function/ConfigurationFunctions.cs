using System.Threading.Tasks;
using Core.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AuthFunctions.Function
{
    public sealed class ConfigurationFunctions
    {
        private readonly IAzureAdb2COptions _options;

        public ConfigurationFunctions(IAzureAdb2COptions options)
        {
            _options = options;
        }

        [FunctionName(nameof(ConfigurationFunction))]
        public IActionResult ConfigurationFunction(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "configuration/redirect")]
            HttpRequest req,
            ILogger log)
        {
            log.LogDebug("{Function} - Start", nameof(ConfigurationFunction));

            var config = new
            {
                RedirectUrl = _options.AuthorizeUrl
            };
            
            log.LogDebug("{Function} - End", nameof(ConfigurationFunction));

            return new OkObjectResult(config);
        }

    }
}

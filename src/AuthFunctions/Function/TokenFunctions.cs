using System.Threading.Tasks;
using System.Xml;
using AuthFunctions.Extensions;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AuthFunctions.Function
{
    public class TokenFunctions
    {
        [FunctionName(nameof(GetToken))]
        public async Task<IActionResult> GetToken(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "token")]
            HttpRequest req,
            ILogger log)
        {
            log.LogDebug("{Function} - Start", nameof(GetToken));

            req.Query.TryGetValue("code", out var code);
            var result = new OkObjectResult(new
            {
                code = code,
                action = "getToken"
            });

            log.LogDebug("{Function} - End", nameof(GetToken));

            return result;
        }

        [FunctionName(nameof(RefreshToken))]
        public async Task<IActionResult> RefreshToken(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "token/refresh")]
            HttpRequest req,
            ILogger log)
        {
            log.LogDebug("{Function} - Start", nameof(RefreshToken));
            
            req.Query.TryGetValue("code", out var code);
            var result = new OkObjectResult(new
            {
                code = code,
                action = "refreshToken"
            });

            log.LogDebug("{Function} - End", nameof(RefreshToken));

            return result;
        }

    }
}

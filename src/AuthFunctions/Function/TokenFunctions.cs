using System.Net;
using System.Threading.Tasks;
using Core.Services.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AuthFunctions.Function
{
    public class TokenFunctions
    {
        private readonly ITokenService _tokenService;

        public TokenFunctions(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [FunctionName(nameof(GetToken))]
        public async Task<IActionResult> GetToken(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "token")]
            HttpRequest req,
            ILogger log)
        {
            log.LogDebug("{Function} - Start", nameof(GetToken));

            var code = "";
            if (req.Headers.ContainsKey("code"))
                code = req.Headers["code"];
            else if (req.Query.ContainsKey("code"))
                code = req.Query["code"];
            else if (req.Form?.ContainsKey("code") ?? false)
                code = req.Form["code"];

            if (string.IsNullOrEmpty(code))
                return new BadRequestObjectResult("unable to find code");

            var result = await _tokenService.GetTokenAsync(code);

            log.LogDebug("{Function} - End", nameof(GetToken));

            return result != null
                ? new OkObjectResult(result)
                : new ObjectResult("error retrieving token") {StatusCode = (int) HttpStatusCode.InternalServerError};
        }

        [FunctionName(nameof(RefreshToken))]
        public async Task<IActionResult> RefreshToken(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "token/refresh")]
            HttpRequest req,
            ILogger log)
        {
            log.LogDebug("{Function} - Start", nameof(RefreshToken));

            var refreshToken = "";
            if (req.Headers.ContainsKey("refresh_token"))
                refreshToken = req.Headers["refresh_token"];
            else if (req.Query.ContainsKey("refresh_token"))
                refreshToken = req.Query["refresh_token"];
            else if (req.Form?.ContainsKey("refresh_token") ?? false)
                refreshToken = req.Form["refresh_token"];

            if (string.IsNullOrEmpty(refreshToken))
                return new BadRequestObjectResult("unable to find refresh_token");

            var result = await _tokenService.RefreshTokenAsync(refreshToken);

            log.LogDebug("{Function} - End", nameof(RefreshToken));

            return result != null
                ? new OkObjectResult(result)
                : new ObjectResult("error refreshing token") {StatusCode = (int) HttpStatusCode.InternalServerError};
        }

    }
}

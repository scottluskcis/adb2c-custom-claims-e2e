using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AuthFunctions.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Security.Core.Extensions;
using Security.Core.Models;
using Security.Core.Services.Token;

namespace AuthFunctions.Function
{
    public class TokenFunctions
    {
        private readonly ITokenValidatorService _tokenValidatorService;
        private readonly ITokenService _tokenService;

        public TokenFunctions(ITokenService tokenService, ITokenValidatorService tokenValidatorService)
        {
            _tokenService = tokenService;
            _tokenValidatorService = tokenValidatorService;
        }

        [FunctionName(nameof(GetToken))]
        public async Task<IActionResult> GetToken(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "token")]
            HttpRequest req,
            ILogger log)
        {
            log.LogDebug("{Function} - Start", nameof(GetToken));

            var code = req.GetValueForKey("code");
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
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "token/refresh")]
            HttpRequest req,
            ILogger log)
        {
            log.LogDebug("{Function} - Start", nameof(RefreshToken));

            var refreshToken = req.GetValueForKey("refresh_token");
            if (string.IsNullOrEmpty(refreshToken))
                return new BadRequestObjectResult("unable to find refresh_token");

            var result = await _tokenService.RefreshTokenAsync(refreshToken);

            log.LogDebug("{Function} - End", nameof(RefreshToken));

            return result != null
                ? new OkObjectResult(result)
                : new ObjectResult("error refreshing token") {StatusCode = (int) HttpStatusCode.InternalServerError};
        }

        [FunctionName(nameof(GetTokenClaims))]
        public IActionResult GetTokenClaims(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "token/claims")]
            HttpRequest req,
            ILogger log)
        {
            log.LogDebug("{Function} - Start", nameof(GetTokenClaims));

            var idToken = req.GetValueForKey("id_token");
            if (string.IsNullOrEmpty(idToken))
                return new BadRequestObjectResult("unable to find id_token");

            var jwtToken = _tokenValidatorService.GetJwtSecurityToken(idToken);
            var result = jwtToken.Claims.GetClaimsInfo();
            
            log.LogDebug("{Function} - End", nameof(GetTokenClaims));

            return result != null
                ? new OkObjectResult(result)
                : new ObjectResult("error retrieving claims") {StatusCode = (int) HttpStatusCode.InternalServerError};
        }
    }
}

using System;
using System.Threading.Tasks;
using AuthFunctions.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Security.Core.Extensions;
using Security.Core.Models;
using Security.Core.Services.Identity;

namespace AuthFunctions.Function
{
    public sealed class IdentityFunctions
    {
        private readonly IIdentityService _identityService;

        public IdentityFunctions(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [FunctionName(nameof(SignUpFunction))]
        public async Task<IActionResult> SignUpFunction(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "identity/signup")]
            HttpRequest req,
            ILogger log)
        {
            log.LogDebug("{Function} - Start", nameof(SignUpFunction));
            
            var result = await ProcessAsync(req, log, IdentityAction.SignUp);

            log.LogDebug("{Function} - End", nameof(SignUpFunction));

            return result;
        }

        [FunctionName(nameof(SignInFunction))]
        public async Task<IActionResult> SignInFunction(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "identity/signin")]
            HttpRequest req,
            ILogger log)
        {
            log.LogDebug("{Function} - Start", nameof(SignInFunction));

            var result = await ProcessAsync(req, log, IdentityAction.SignIn);

            log.LogDebug("{Function} - End", nameof(SignInFunction));

            return result;
        }

        private async Task<IActionResult> ProcessAsync(HttpRequest req, ILogger log, IdentityAction action)
        {
            IActionResult result;
            try
            {
                var inputClaimsModel = await req.ReadContentAsync<InputClaimsModel>();

                log.LogDebug("InputClaims: {Json}", inputClaimsModel.ToJson(Formatting.Indented));

                var signUpResult = action == IdentityAction.SignUp
                    ? await _identityService.SignUpAsync(inputClaimsModel)
                    : await _identityService.SignInAsync(inputClaimsModel);

                log.LogDebug("OutputClaims: {Json}", signUpResult.ToJson(Formatting.Indented));

                result = new OkObjectResult(signUpResult);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "error occurred");
                result = ex.AsActionResult();
            }
            return result;
        }
    }
}

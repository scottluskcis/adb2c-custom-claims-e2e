using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Security.Core.Models;

namespace Security.Core.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly ILogger _logger;

        public IdentityService(ILogger<IdentityService> logger)
        {
            _logger = logger;
        }

        public Task<OutputClaimsModel> SignUpAsync(InputClaimsModel model)
        {
            var result = Process(model, IdentityAction.SignUp);
            return Task.FromResult(result);
        }

        public Task<OutputClaimsModel> SignInAsync(InputClaimsModel model)
        {
            var result = Process(model, IdentityAction.SignIn);
            return Task.FromResult(result);
        }

        private OutputClaimsModel Process(InputClaimsModel model, IdentityAction action)
        {
            _logger.LogInformation($"{nameof(IdentityService)}.{nameof(Process)} - Start");

            var isValid = action != IdentityAction.SignUp || ValidateInputClaims(model);
            
            if (!isValid)
                throw new ArgumentException("Validation failed for InputClaims.");

            var inputClaimsJson = JsonConvert.SerializeObject(model, Formatting.None);

            var outputClaims = new OutputClaimsModel
            {
                LoyaltyNumber = new Random().Next(100, 1000).ToString(),
                Action = $"{action.ToString()} at {DateTimeOffset.UtcNow.Ticks}"
            };

            _logger.LogInformation($"{nameof(IdentityService)}.{nameof(Process)} - End");

            return outputClaims;
        }

        private bool ValidateInputClaims(InputClaimsModel inputClaims)
        {
            // Run a simple input validation
            if (inputClaims.FirstName.ToLower() == "test")
                throw new ArgumentException("Test name is not valid, please provide a valid name");

            // validation passes
            return true;
        }

    }
}

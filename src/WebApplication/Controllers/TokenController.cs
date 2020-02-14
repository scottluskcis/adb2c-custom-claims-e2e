using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Security.Core.Extensions;
using Security.Core.Models;

namespace WebApplication.Controllers
{
    public class TokenController : Controller
    {
        private readonly string _expectedState = $"from_mvc_app_{nameof(SignInController)}";

        private readonly HttpClient _client;
        private readonly ILogger<SignInController> _logger;

        public TokenController(IHttpClientFactory clientFactory, ILogger<SignInController> logger)
        {
            _client = clientFactory.CreateClient(Startup.AuthFunctionApiClientName);
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            if (!Request.Query.TryGetValue("code", out var code))
                return RedirectToAction("Error", "Home");
            if (!Request.Query.TryGetValue("state", out var state) && state[0] != _expectedState)
                return RedirectToAction("Error", "Home");

            var response = await _client.GetAsync($"token?code={code[0]}");
            response.EnsureSuccessStatusCode();

            var contentString = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("response from endpoint: {response}", contentString);

            var result = contentString.FromJson<TokenResponse>();
            return View("TokenDetail", result);
        }
        
        public IActionResult TokenDetail()
        {
            return View();
        }

        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            if(string.IsNullOrEmpty(refreshToken))
                return RedirectToAction("Error", "Home");

            var response = await _client.GetAsync($"token/refresh?refresh_token={refreshToken}");
            response.EnsureSuccessStatusCode();

            var contentString = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("response from endpoint: {response}", contentString);

            var result = contentString.FromJson<TokenResponse>();
            return View("TokenDetail", result);
        }

        public async Task<IActionResult> Claims(string token)
        {
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Error", "Home");

            var response = await _client.GetAsync($"token/claims?id_token={token}");
            response.EnsureSuccessStatusCode();

            var contentString = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("response from endpoint: {response}", contentString);

            var result = contentString.FromJson<IEnumerable<ClaimInfo>>();
            return View(result);
        }

    }
}
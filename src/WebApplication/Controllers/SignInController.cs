using System.Net.Http;
using System.Threading.Tasks;
using CustomClaims.Core.Extensions;
using CustomClaims.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class SignInController : Controller
    {
        private readonly string _expectedState = $"from_mvc_app_{nameof(SignInController)}";

        private readonly HttpClient _client;
        private readonly ILogger<SignInController> _logger;

        public SignInController(IHttpClientFactory clientFactory, ILogger<SignInController> logger)
        {
            _client = clientFactory.CreateClient(Startup.AuthFunctionApiClientName);
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync("configuration/redirect");
            response.EnsureSuccessStatusCode();

            var contentString = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("response from endpoint: {response}", contentString);

            var result = contentString.FromJson<AuthRedirectModel>();
            result.State = _expectedState;
            result.LocalRedirectUrl = GetLocalRedirectUrl();

            return View(result);
        }

        public async Task<IActionResult> Token()
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

        private string GetLocalRedirectUrl()
        {
            return $"{Request.Scheme}://{Request.Host.Value}/SignIn/Token";
        }
    }
}
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Security.Core.Extensions;
using Security.Core.Models;
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

        private string GetLocalRedirectUrl()
        {
            return $"{Request.Scheme}://{Request.Host.Value}/Token/Index";
        }
    }
}
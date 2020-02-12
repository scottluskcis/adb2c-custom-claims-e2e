using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Extensions;
using Core.Models;
using Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly HttpClient _client;
        private readonly AzureAdb2cOptions _options;
        private readonly ILogger _logger;

        public TokenService(HttpClient client, IOptions<AzureAdb2cOptions> options, ILogger<TokenService> logger)
        {
            _client = client;
            _options = options?.Value;
            _logger = logger;
        }

        public async Task<TokenResponse> GetTokenAsync(string code, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("{Function} - Start", nameof(GetTokenAsync));

            var url = $"{_options.B2CUrl}/token?p={_options.DefaultPolicy}";

            var now = DateTimeOffset.UtcNow.Ticks.ToString();

            var formData = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    {"client_id", _options.ClientId},
                    {"client_secret", _options.ClientSecret},
                    {"scope", _options.ApiScopes},
                    {"redirect_uri", _options.RedirectUri},
                    {"grant_type", "authorization_code"},
                    {"code", code},
                    {"state", now}
                });

            var response = await _client.PostAsync(url, formData, cancellationToken);
            response.EnsureSuccessStatusCode();

            var contentString = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("response from endpoint: {response}", contentString);

            var result = contentString.FromJson<TokenResponse>();
            
            _logger.LogInformation("{Function} - End", nameof(GetTokenAsync));

            return result;
        }

        public Task RefreshTokenAsync()
        {
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Extensions;
using Core.Models;
using Core.Options;
using Microsoft.Extensions.Logging;

namespace Core.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly HttpClient _client;
        private readonly IAzureAdb2COptions _options;
        private readonly ILogger _logger;

        public TokenService(HttpClient client, IAzureAdb2COptions options, ILogger<TokenService> logger)
        {
            _client = client;
            _options = options;
            _logger = logger;
        }

        public async Task<TokenResponse> GetTokenAsync(string code, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("{Function} - Start", nameof(GetTokenAsync));

            if (!(_options?.IsValid ?? false))
            {
                _logger.LogError($"invalid {nameof(IAzureAdb2COptions)} settings. {_options?.ErrorMessage}");
                throw new InvalidOperationException($"invalid configuration, unable to perform operation. {_options?.ErrorMessage}");
            }

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

        public  async Task<TokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("{Function} - Start", nameof(RefreshTokenAsync));

            if (!(_options?.IsValid ?? false))
            {
                _logger.LogError($"invalid {nameof(IAzureAdb2COptions)} settings. {_options?.ErrorMessage}");
                throw new InvalidOperationException($"invalid configuration, unable to perform operation. {_options?.ErrorMessage}");
            }

            var url = $"{_options.B2CUrl}/token?p={_options.DefaultPolicy}";

            var now = DateTimeOffset.UtcNow.Ticks.ToString();

            var formData = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    {"client_id", _options.ClientId},
                    {"client_secret", _options.ClientSecret},
                    {"scope", _options.ApiScopes},
                    {"redirect_uri", _options.RedirectUri},
                    {"grant_type", "refresh_token"},
                    {"refresh_token", refreshToken },
                    {"state", now}
                });

            var response = await _client.PostAsync(url, formData, cancellationToken);
            response.EnsureSuccessStatusCode();

            var contentString = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("response from endpoint: {response}", contentString);

            var result = contentString.FromJson<TokenResponse>();
            
            _logger.LogInformation("{Function} - End", nameof(RefreshTokenAsync));

            return result;
        }
    }
}

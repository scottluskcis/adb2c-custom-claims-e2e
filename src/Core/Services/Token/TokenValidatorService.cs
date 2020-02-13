using CustomClaims.Core.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace CustomClaims.Core.Services.Token
{
    public sealed class TokenValidatorService : ITokenValidatorService
    {
        private readonly ITokenValidationOptions _options;
        private readonly ILogger _logger;

        public TokenValidatorService(ITokenValidationOptions options, ILogger<TokenValidatorService> logger)
        {
            _options = options;
            _logger = logger;
        }

        public JwtSecurityToken GetJwtSecurityToken(string token)
        {
            _logger.LogInformation("{Function} - Start", nameof(GetJwtSecurityToken));

            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));

            _logger.LogDebug("retrieve TokenValidationParameters");
            var parameters = GetTokenValidationParameters(_options);
            
            _logger.LogDebug("validate security token");
            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(token, parameters, out var validSecurityToken);

            _logger.LogDebug("cast validated token a JwtSecurityToken");
            var jwtToken = validSecurityToken as JwtSecurityToken;

            _logger.LogInformation("{Function} - End", nameof(GetJwtSecurityToken));

            return jwtToken;
        }

        private TokenValidationParameters GetTokenValidationParameters(ITokenValidationOptions options)
        {
            if (!(options?.IsValid ?? false))
                throw new ArgumentException($"invalid {nameof(ITokenValidationOptions)} were provided. {options?.ErrorMessage}");

            var validationParameters = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidAudience = options.ClientId,
                ValidateAudience = true,
                ValidIssuer = options.Issuer,
                ValidateIssuer = true,
                ValidateLifetime = true,
                IssuerSigningKey = GetRsaSecurityKey(options.RsaModulus, options.RsaExponent)
            };

            return validationParameters;
        }

        private RsaSecurityKey GetRsaSecurityKey(string rsaModulus, string rsaExponent)
        {
            var provider = GetRsaCryptoServiceProvider(rsaModulus, rsaExponent);
            var key = new RsaSecurityKey(provider);
            return key;
        }

        private RSACryptoServiceProvider GetRsaCryptoServiceProvider(string rsaModulus, string rsaExponent)
        {            
            var rsa = new RSACryptoServiceProvider();

            rsa.ImportParameters(new RSAParameters
            {
                Modulus = FromBase64Url(rsaModulus),
                Exponent = FromBase64Url(rsaExponent)
            });

            return rsa;
        }

        private byte[] FromBase64Url(string base64Url)
        {
            var padded = base64Url.Length % 4 == 0
                ? base64Url
                : base64Url + "====".Substring(base64Url.Length % 4);

            var base64 = padded.Replace("_", "/")
                .Replace("-", "+");

            return Convert.FromBase64String(base64);
        }
    }
}

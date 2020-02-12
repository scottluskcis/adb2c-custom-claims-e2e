using Newtonsoft.Json;

namespace CustomClaims.Core.Models
{
    public class TokenResponse
    {
        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("not_before")]
        public long? NotBefore { get; set; }

        [JsonProperty("id_token_expires_in")]
        public long? IdTokenExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("refresh_token_expires_in")]
        public int RefreshTokenExpiresIn { get; set; }
    }
}

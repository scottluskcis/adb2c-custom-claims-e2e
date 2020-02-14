using Newtonsoft.Json;

namespace WebApplication.Models
{
    public class AuthRedirectModel
    {
        [JsonProperty("redirectUrl")]
        public string B2CUrl { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("redirect_uri")]
        public string LocalRedirectUrl { get; set; }

        public string FullUrl => $"{B2CUrl}&redirect_uri={LocalRedirectUrl}&state={State}&campaignId=mvc_app";
    }
}

using System.Collections.Generic;

namespace CustomClaims.Core.Options
{
    public class AzureAdb2COptions : IAzureAdb2COptions 
    {
        public const string PolicyAuthenticationProperty = "Policy";

        public string ClientId { get; set; }
        public string AzureAdB2CInstance { get; set; }
        public string Tenant { get; set; }
        public string SignUpSignInPolicyId { get; set; }
        public string ResetPasswordPolicyId { get; set; }
        public string EditProfilePolicyId { get; set; }
        public string RedirectUri { get; set; }

        public string DefaultPolicy => SignUpSignInPolicyId;
        public string Authority => $"{AzureAdB2CInstance}/{Tenant}/{DefaultPolicy}/v2.0";
        public string B2CUrl => $"https://{Tenant}.b2clogin.com/{Tenant}.onmicrosoft.com/oauth2/v2.0";
        public string AuthorizeUrl => $"{B2CUrl}/authorize?p={SignUpSignInPolicyId}&client_id={ClientId}&response_type=code&scope={ApiScopes}&prompt=login";

        public string ClientSecret { get; set; }
        public string ApiUrl { get; set; }
        public string ApiScopes { get; set; }

        public bool IsValid => string.IsNullOrEmpty(ErrorMessage);

        public string ErrorMessage => string.Join(" ", Validate()).Trim();

        public string[] Validate()
        {
            var errors = new List<string>();

            if(string.IsNullOrEmpty(ClientId))
                errors.Add($"{nameof(ClientId)} is required.");

            if(string.IsNullOrEmpty(Tenant))
                errors.Add($"{nameof(Tenant)} is required.");

            if(string.IsNullOrEmpty(ClientSecret))
                errors.Add($"{nameof(ClientSecret)} is required.");

            if(string.IsNullOrEmpty(DefaultPolicy))
                errors.Add($"{nameof(DefaultPolicy)} is required.");

            if(string.IsNullOrEmpty(ApiScopes))
                errors.Add($"{nameof(ApiScopes)} is required.");

            return errors.ToArray();
        }
    }
}

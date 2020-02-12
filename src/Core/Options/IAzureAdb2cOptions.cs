namespace CustomClaims.Core.Options
{
    public interface IAzureAdb2COptions
    {
        string ClientId { get; }
        string AzureAdB2CInstance { get; }
        string Tenant { get; }
        string SignUpSignInPolicyId { get; }
        string ResetPasswordPolicyId { get; }
        string EditProfilePolicyId { get; }
        string RedirectUri { get; }
        string DefaultPolicy { get; }
        string Authority { get; }
        string B2CUrl { get; }
        string ClientSecret { get; }
        string ApiUrl { get; }
        string ApiScopes { get; }
        bool IsValid { get; }
        string ErrorMessage { get; }
        string AuthorizeUrl { get; }
    }
}
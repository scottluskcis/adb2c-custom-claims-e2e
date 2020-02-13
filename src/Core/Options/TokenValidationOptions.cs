using System.Collections.Generic;

namespace CustomClaims.Core.Options
{
    public class TokenValidationOptions : ITokenValidationOptions 
    {
        public string ClientId { get; set; }
        public string Issuer { get; set; }
        public string RsaModulus { get; set; }
        public string RsaExponent { get; set; }

        public bool IsValid => string.IsNullOrEmpty(ErrorMessage);

        public string ErrorMessage => string.Join(" ", Validate()).Trim();

        public string[] Validate()
        {
            var errors = new List<string>();

            if(string.IsNullOrEmpty(ClientId))
                errors.Add($"{nameof(ClientId)} is required.");

            if(string.IsNullOrEmpty(Issuer))
                errors.Add($"{nameof(Issuer)} is required.");

            if(string.IsNullOrEmpty(RsaModulus))
                errors.Add($"{nameof(RsaModulus)} is required.");
            
            if(string.IsNullOrEmpty(RsaExponent))
                errors.Add($"{nameof(RsaExponent)} is required.");

            return errors.ToArray();
        }
    }
}

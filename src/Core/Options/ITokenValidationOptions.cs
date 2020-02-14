namespace Security.Core.Options
{
    public interface ITokenValidationOptions
    {
        string ClientId { get;  }
        string Issuer { get;  }
        string RsaModulus { get;  }
        string RsaExponent { get;  }
        bool IsValid { get; }
        string ErrorMessage { get; }
    }
}
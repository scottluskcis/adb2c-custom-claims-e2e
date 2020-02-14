using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Security.Core.Models;

namespace Security.Core.Extensions
{
    public static class ClaimsExtensions
    {
        public static IEnumerable<ClaimInfo> GetClaimsInfo(this IEnumerable<Claim> claims)
        {
            return claims
                .Select(s => new ClaimInfo
                {
                    Type = s.Type,
                    Value = s.Value
                });
        }
    }
}

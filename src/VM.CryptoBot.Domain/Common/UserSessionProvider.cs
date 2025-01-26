using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using VM.CryptoBot.Domain.Repositories;

namespace VM.CryptoBot.Domain.Common;

public class UserSessionProvider(IHttpContextAccessor httpContextAccessor) : IUserSessionProvider
{
    public ClaimsPrincipal GetUser()
    {
        return httpContextAccessor.HttpContext.User;
    }

    public IEnumerable<Claim> GetUserClaims()
    {
        return GetUser().Claims;
    }

    public Claim? GetTypedUserClaim(string claimType)
    {
        return GetUserClaims().FirstOrDefault(claim => claim.Type == claimType);
    }

    public string? GetUserEmail()
    {
        return GetTypedUserClaim(ClaimTypes.Email)?.Value;
    }
}
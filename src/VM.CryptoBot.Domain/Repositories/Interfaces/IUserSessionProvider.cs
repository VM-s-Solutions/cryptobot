using System.Security.Claims;

namespace VM.CryptoBot.Domain.Repositories.Interfaces;

public interface IUserSessionProvider
{
    ClaimsPrincipal GetUser();

    IEnumerable<Claim> GetUserClaims();

    Claim? GetTypedUserClaim(string claimType);

    string? GetUserEmail();
}
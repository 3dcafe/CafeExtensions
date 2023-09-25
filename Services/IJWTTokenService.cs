using CafeExtensions.Interfaces;

namespace CafeExtensions.Services;
/// <summary>
/// Base interface for token issuance service.
/// </summary>
public interface IJWTTokenService
{
    /// <summary>
    /// Get a JWT security token - a base method that should be implemented.
    /// </summary>
    /// <param name="user">The user for whom the token is generated.</param>
    /// <returns>The JWT security token.</returns>
    string GetJwtSecurityToken(IUser user);
}

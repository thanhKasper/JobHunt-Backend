using JobHunt.Core.Domain.Entities;
using JobHunt.Core.DTO;

namespace JobHunt.Core.ServiceContracts;

public interface IJwtService
{
    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the token is generated.</param>
    /// <returns>An AuthenticationResponse containing the JWT token and expiration.</returns>
    AuthenticationResponse GenerateToken(JobHunter user);
}
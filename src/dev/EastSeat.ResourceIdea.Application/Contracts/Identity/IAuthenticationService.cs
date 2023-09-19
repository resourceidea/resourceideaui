using EastSeat.ResourceIdea.Application.Models;

namespace EastSeat.ResourceIdea.Application.Contracts.Identity;

/// <summary>
/// Authentication service.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Handles authentication requests.
    /// </summary>
    /// <param name="request">Authentication request.</param>
    /// <returns>Authentication response.</returns>
    Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);

    Task<UserRegistrationResponse> RegisterUserAsync(UserRegistrationRequest request);
}

﻿using EastSeat.ResourceIdea.Application.Models;

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

    /// <summary>
    /// Delete user from the system.
    /// </summary>
    /// <param name="userId">Id of user to be deleted.</param>
    /// <returns></returns>
    Task DeleteUserAsync(Guid userId);

    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="request">User registration request.</param>
    /// <returns>User registration response.</returns>
    Task<UserRegistrationResponse> RegisterUserAsync(UserRegistrationRequest request);
}
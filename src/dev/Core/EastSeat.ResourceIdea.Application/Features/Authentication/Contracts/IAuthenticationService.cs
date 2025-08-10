// ----------------------------------------------------------------------------------
// File: IAuthenticationService.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Authentication\Contracts\IAuthenticationService.cs
// Description: Interface for authentication operations.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Authentication.Models;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Authentication.Contracts;

/// <summary>
/// Interface for authentication operations.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Authenticates a user with email and password.
    /// </summary>
    /// <param name="email">User's email address</param>
    /// <param name="password">User's password</param>
    /// <param name="rememberMe">Whether to persist the login session</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication result</returns>
    Task<ResourceIdeaResponse<LoginResultModel>> LoginAsync(
        string email, 
        string password, 
        bool rememberMe, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Signs out the current user.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Logout result</returns>
    Task<ResourceIdeaResponse<LogoutResultModel>> LogoutAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates that an authenticated user has all required claims.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the user has valid claims, false otherwise</returns>
    Task<ResourceIdeaResponse<UserValidationResult>> ValidateUserClaimsAsync(CancellationToken cancellationToken = default);
}
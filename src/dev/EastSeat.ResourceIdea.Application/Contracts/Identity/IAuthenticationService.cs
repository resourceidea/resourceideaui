using EastSeat.ResourceIdea.Application.Features.ApplicationUser.Commands.CreateApplicationUser;
using EastSeat.ResourceIdea.Application.Models;
using EastSeat.ResourceIdea.Application.Responses;

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
    //Task<ApiAuthenticationResponse> AuthenticateApiUserAsync(AuthenticationRequest request);

    /// <summary>
    /// Handles authentication of a web user.
    /// </summary>
    /// <param name="request">Authentication request.</param>
    /// <returns>Authentication response.</returns>
    Task<BaseResponse<ApplicationUserViewModel>> AuthenticateUserAsync(AuthenticationRequest request);

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
    Task<BaseResponse<CreateApplicationUserViewModel>> RegisterUserAsync(UserRegistrationRequest request);

    /// <summary>
    /// Get the application user given the Id.
    /// </summary>
    /// <param name="id">Application user Id.</param>
    /// <returns>Operation response with application user as content.</returns>
    Task<BaseResponse<ApplicationUserViewModel>> GetApplicationUserAsync(Guid id);
}

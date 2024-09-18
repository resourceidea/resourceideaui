using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Users.Models;

namespace EastSeat.ResourceIdea.Application.Features.ApplicationUsers.Contracts;

/// <summary>
/// User authentication service contract.
/// </summary>
public interface IUserAuthenticationService
{
    /// <summary>
    /// Login a user.
    /// </summary>
    /// <param name="loginModel">Login request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<ResourceIdeaResponse<LoginModel>> LoginAsync(LoginModel loginModel, CancellationToken cancellationToken);
}

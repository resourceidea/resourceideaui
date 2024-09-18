using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Users.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.ApplicationUsers.Commands;

/// <summary>
/// Command for logging in a user.
/// </summary>
public class LoginCommand : IRequest<ResourceIdeaResponse<LoginModel>>
{
    /// <summary>Email of the user.</summary>
    public required string Email { get; set; }

    /// <summary>Password of the user.</summary>
    public required string Password { get; set; }
}

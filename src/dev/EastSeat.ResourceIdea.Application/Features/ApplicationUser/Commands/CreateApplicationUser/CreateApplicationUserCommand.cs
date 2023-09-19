using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.ApplicationUser.Commands.CreateApplicationUser;

/// <summary>
/// Command to create an application user.
/// </summary>
public class CreateApplicationUserCommand : IRequest<CreateApplicationUserCommandResponse>
{
    /// <summary>Application user's first name.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Application user's last name.</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>Application user's email.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Application user's password.</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>UserId of the subscription to which the application user belongs.</summary>
    public Guid SubscriptionId { get; set; } = Guid.Empty;


}

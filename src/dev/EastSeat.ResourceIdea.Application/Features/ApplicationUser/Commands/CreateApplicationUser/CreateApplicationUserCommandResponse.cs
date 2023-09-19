using EastSeat.ResourceIdea.Application.Responses;

namespace EastSeat.ResourceIdea.Application.Features.ApplicationUser.Commands.CreateApplicationUser;

/// <summary>
/// Response to the command to create an application user.
/// </summary>
public class CreateApplicationUserCommandResponse : BaseResponse
{
    /// <summary>
    /// Instantiates <see cref="CreateApplicationUserCommandResponse"/>.
    /// </summary>
    public CreateApplicationUserCommandResponse() : base()
    {
    }

    /// <summary>
    /// Instantiates <see cref="CreateApplicationUserCommandResponse"/>.
    /// </summary>
    /// <param name="message">Response message.</param>
    public CreateApplicationUserCommandResponse(string message) : base(message)
    {
    }

    /// <summary>
    /// Instantiates <see cref="CreateApplicationUserCommandResponse"/>.
    /// </summary>
    /// <param name="success">Flag indicating whether execution of the command was a success or not.</param>
    /// <param name="message">Response message.</param>
    public CreateApplicationUserCommandResponse(bool success, string message) : base(success, message)
    {
    }

    /// <summary>New application user that has been created.</summary>
    public CreateApplicationUserViewModel ApplicationUser { get; set; } = default!;
}

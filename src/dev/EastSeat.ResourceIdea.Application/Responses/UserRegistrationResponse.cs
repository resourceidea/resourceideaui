using EastSeat.ResourceIdea.Application.Features.ApplicationUser.Commands.CreateApplicationUser;

namespace EastSeat.ResourceIdea.Application.Responses;

/// <summary>
/// Represents response returned after registering a user.
/// </summary>
public class UserRegistrationResponse : BaseResponse
{
    /// <summary>
    /// Instantiates <see cref="UserRegistrationResponse"/>.
    /// </summary>
    public UserRegistrationResponse() : base()
    {
    }

    /// <summary>
    /// Instantiates <see cref="UserRegistrationResponse"/>.
    /// </summary>
    /// <param name="message">Response message.</param>
    public UserRegistrationResponse(string message) : base(message)
    {
    }

    /// <summary>
    /// Instantiates <see cref="UserRegistrationResponse"/>.
    /// </summary>
    /// <param name="success">Indicates where the command execution was a success.</param>
    /// <param name="message">Response message.</param>
    public UserRegistrationResponse(bool success, string message) : base(success, message)
    {
    }

    /// <summary>
    /// Application User.
    /// </summary>
    public CreateApplicationUserViewModel ApplicationUser { get; set; } = default!;
}










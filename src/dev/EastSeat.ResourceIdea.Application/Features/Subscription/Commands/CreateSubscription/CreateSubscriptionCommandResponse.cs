using EastSeat.ResourceIdea.Application.Features.ApplicationUser.Commands.CreateApplicationUser;
using EastSeat.ResourceIdea.Application.Features.Employee.Commands.CreateEmployee;
using EastSeat.ResourceIdea.Application.Responses;

namespace EastSeat.ResourceIdea.Application.Features.Subscription.Commands.CreateSubscription;

/// <summary>
/// Response from the command used to create a subscription.
/// </summary>
public class CreateSubscriptionCommandResponse : BaseResponse
{
    /// <summary>
    /// Instantiates <see cref="CreateSubscriptionCommandResponse"/>.
    /// </summary>
    public CreateSubscriptionCommandResponse() : base()
    {
    }

    /// <summary>
    /// Instantiates <see cref="CreateSubscriptionCommandResponse"/>.
    /// </summary>
    /// <param name="message">Response message.</param>
    public CreateSubscriptionCommandResponse(string message) : base(message)
    {
    }

    /// <summary>
    /// Instantiates <see cref="CreateSubscriptionCommandResponse"/>.
    /// </summary>
    /// <param name="success">Indicates where the command execution was a success.</param>
    /// <param name="message">Response message.</param>
    public CreateSubscriptionCommandResponse(bool success, string message) : base(success, message)
    {
    }

    /// <summary>
    /// Subscription that has been created.
    /// </summary>
    public CreateSubscriptionViewModel Subscription { get; set; } = default!;

    /// <summary>
    /// Application user that has been created.
    /// </summary>
    public CreateApplicationUserViewModel ApplicationUser { get; set; } = default!;

    /// <summary>
    /// Employee that has been created.
    /// </summary>
    public CreateEmployeeViewModel Employee { get; set; } = default!;
}

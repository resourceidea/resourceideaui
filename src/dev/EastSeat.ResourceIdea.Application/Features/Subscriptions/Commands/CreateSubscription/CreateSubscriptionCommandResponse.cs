using EastSeat.ResourceIdea.Application.Features.ApplicationUsers.Commands.CreateApplicationUser;
using EastSeat.ResourceIdea.Application.Features.Employees.Commands.CreateEmployee;
using EastSeat.ResourceIdea.Application.Responses;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands.CreateSubscription;

/// <summary>
/// Response from the command used to create a subscription.
/// </summary>
public class CreateSubscriptionCommandResponse : BaseResponse<CreateSubscriptionViewModel>
{
    /// <summary>
    /// Application user that has been created.
    /// </summary>
    public CreateApplicationUserViewModel ApplicationUser { get; set; } = default!;

    /// <summary>
    /// Employee that has been created.
    /// </summary>
    public CreateEmployeeViewModel Employee { get; set; } = default!;
}

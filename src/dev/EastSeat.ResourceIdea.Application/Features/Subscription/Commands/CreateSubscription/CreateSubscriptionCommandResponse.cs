using EastSeat.ResourceIdea.Application.Responses;

namespace EastSeat.ResourceIdea.Application.Features.Subscription.Commands.CreateSubscription;

/// <summary>
/// Response from the command used to create a subscription.
/// </summary>
public class CreateSubscriptionCommandResponse : BaseResponse
{
    public CreateSubscriptionCommandResponse() : base()
    {
    }

    /// <summary>
    /// Subscription that has been created.
    /// </summary>
    public CreateSubscriptionVM Subscription { get; set; } = default!;
}

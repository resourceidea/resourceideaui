namespace EastSeat.ResourceIdea.Application.Features.Subscription.Commands.CreateSubscription;

/// <summary>
/// View model for the subscription that has been created.
/// </summary>
public class CreateSubscriptionVM
{
    /// <summary>Subscription ID.</summary>
    public Guid SubscriptionId { get; set; } = Guid.Empty;

    /// <summary>Subscriber's name.</summary>
    public string SubscriberName { get; set; } = string.Empty;

    /// <summary>Subscription start date.</summary>
    public DateTime StartDate { get; set; }
}

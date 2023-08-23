namespace EastSeat.ResourceIdea.Application.Features.Subscription.Queries.GetSubscriptionsList;

/// <summary>
/// View model for the subscriptions list.
/// </summary>
public class SubscriptionsListVM
{
    /// <summary>Subscription ID.</summary>
    public Guid SubscriptionId { get; set; } = Guid.Empty;
    ///    
    /// <summary>Subscriber's name.</summary>
    public string SubscriberName { get; set; } = string.Empty;
    ///    
    /// <summary>Subscription start date.</summary>
    public DateTime StartDate { get; set; }
}

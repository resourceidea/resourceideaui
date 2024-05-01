using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Enums;

namespace EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;

/// <summary>
/// Empty subscription instance.
/// </summary>
public sealed record EmptySubscription
{
    public static Subscription Instance => new()
    {
        Id = SubscriptionId.Empty,
        Status = SubscriptionStatus.Inactive,
        SubscriptionDate = null,
        SubscriptionCancellationDate = null
    };

    private EmptySubscription() { }
}
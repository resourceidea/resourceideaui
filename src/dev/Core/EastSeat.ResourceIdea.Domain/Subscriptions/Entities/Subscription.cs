using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Enums;
using EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Subscriptions.Entities;

/// <summary>
/// Subscription entity.
/// </summary>
public class Subscription : BaseEntity
{
    /// <summary>
    /// Subscription unique identifier.
    /// </summary>
    public SubscriptionId Id { get; set; }

    /// <summary>
    /// Date when the subscription was made by the tenant.
    /// </summary>
    public DateTimeOffset? SubscriptionDate { get; set; }

    /// <summary>
    /// Status of the subscription.
    /// </summary>
    public SubscriptionStatus Status { get; set; }

    /// <summary>
    /// Date when the subscription was cancelled by the tenant.
    /// </summary>
    public DateTimeOffset? SubscriptionCancellationDate { get; set; }
}
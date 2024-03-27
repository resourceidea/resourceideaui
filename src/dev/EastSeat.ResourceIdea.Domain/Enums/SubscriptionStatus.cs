namespace EastSeat.ResourceIdea.Domain.Enums;

/// <summary>
/// Subscription status.
/// </summary>
public enum SubscriptionStatus
{
    /// <summary>
    /// Subscription is active.
    /// </summary>
    Active,

    /// <summary>
    /// Subscription has been suspended.
    /// </summary>
    Suspended,

    /// <summary>
    /// Subscription has expired.
    /// </summary>
    Expired,

    /// <summary>
    /// Subscription has been cancelled.
    /// </summary>
    Cancelled
}

namespace EastSeat.ResourceIdea.Domain.SubscriptionServiceManagement.ValueObjects;

/// <summary>
/// Status of a subscribed service.
/// </summary>
public enum SubscriptionServiceStatus
{
    /// <summary>Service is active.</summary>
    Active,

    /// <summary>Service is terminated.</summary>
    EndOfLife,

    /// <summary>Service is retired.</summary>
    Retired,
}

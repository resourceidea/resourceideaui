using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;

/// <summary>
/// Subscribed service entity.
/// </summary>
public class SubscriptionService : BaseEntity
{
    /// <summary>Service Id.</summary>
    public SubscriptionServiceId Id { get; set; }

    /// <summary>Service name.</summary>
    public string Name { get; set; } = string.Empty;
}
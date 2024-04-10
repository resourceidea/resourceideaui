using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;

public record SubscriptionServiceModel
{
    /// <summary>Service Id.</summary>
    public SubscriptionServiceId Id { get; set; }

    /// <summary>Service name.</summary>
    public string Name { get; set; } = string.Empty;
}
using EastSeat.ResourceIdea.Domain.SubscriptionServiceManagement.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.SubscriptionServiceManagement.Models;

public class SubscriptionServiceModel
{
    /// <summary>Service Id.</summary>
    public SubscriptionServiceId Id { get; set; }

    /// <summary>Service name.</summary>
    public string Name { get; set; } = string.Empty;
}
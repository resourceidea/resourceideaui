using EastSeat.ResourceIdea.Domain.Common;

namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Subscriber's client profile.
/// </summary>
public class Client : BaseSubscriptionEntity
{
    /// <summary>Client ID.</summary>
    public Guid Id { get; set; } = Guid.Empty;

    /// <summary>Client name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Client physical address.</summary>
    public string? Address { get; set; }

    /// <summary>Color code to identify client engagements.</summary>
    public string? ColorCode { get; set; }

    /// <summary>Subscription to which the client belongs to.</summary>
    public Subscription? Subscription { get; set; }

    /// <summary>Engagements with the client.</summary>
    public IEnumerable<Engagement>? Engagements { get; set; }
}

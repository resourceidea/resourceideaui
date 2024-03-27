using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Persistence.Models;

/// <summary>
/// Client entity.
/// </summary>
public class ClientEntity : BaseEntity
{
    /// <summary>Client id.</summary>
    public Guid Id { get; set; }

    /// <summary>Client name.</summary>
    public string? Name { get; set; }

    /// <summary>Client's physical address.</summary>
    public string? Address { get; set; }

    /// <summary>Color code to identify client engagements.</summary>
    public string? ColorCode { get; set; }

    /// <summary>Subscription to which the client belongs to.</summary>
    public SubscriptionEntity? Subscription { get; set; }
}

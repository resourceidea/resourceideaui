using EastSeat.ResourceIdea.Domain.Common;

namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Subscribing entity asset.
/// </summary>
public class Asset : BaseSubscriptionEntity
{
    /// <summary>Asset ID.</summary>
    public Guid Id { get; set; } = Guid.Empty;

    /// <summary>Asset description.</summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>Subscription to which the asset belongs.</summary>
    public Subscription? Subscription { get; set; }

    /// <summary>Assignments of the asset.</summary>
    public IEnumerable<AssetAssignment>? Assignments { get; set; }
}

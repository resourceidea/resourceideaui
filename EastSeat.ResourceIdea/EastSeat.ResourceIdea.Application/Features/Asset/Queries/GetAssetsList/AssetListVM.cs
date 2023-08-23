namespace EastSeat.ResourceIdea.Application.Features.Asset.Queries.GetAssetsList;

/// <summary>
/// View model for the list of assets record.
/// </summary>
public class AssetListVM
{
    /// <summary>Asset Id.</summary>
    public Guid Id { get; set; } = Guid.Empty;

    /// <summary>Asset description.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Subscription Id.</summary>
    public Guid SubscriptionId { get; set; } = Guid.Empty;
}

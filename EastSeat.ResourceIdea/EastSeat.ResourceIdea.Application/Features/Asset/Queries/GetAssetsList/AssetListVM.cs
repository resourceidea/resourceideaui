namespace EastSeat.ResourceIdea.Application.Features.Asset.Queries.GetAssetsList;

/// <summary>
/// View model for the list of assets record.
/// </summary>
public class AssetListVM
{
    /// <summary>
    /// Asset ID.
    /// </summary>
    public Guid AssetId { get; set; } = Guid.Empty;

    /// <summary>
    /// Asset description.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

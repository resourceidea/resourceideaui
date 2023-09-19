namespace EastSeat.ResourceIdea.Application.Features.Asset.Commands;

/// <summary>
/// Data Transfer Object for an asset that has been created.
/// </summary>
public class CreateAssetDTO
{
    /// <summary>Asset ID.</summary>
    public Guid? Id { get; set; }

    /// <summary>Asset description.</summary>
    public string? Description { get; set; }
}

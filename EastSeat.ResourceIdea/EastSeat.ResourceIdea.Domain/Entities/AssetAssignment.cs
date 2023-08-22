namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Asset assignment.
/// </summary>
public class AssetAssignment : Assignment
{
    /// <summary>
    /// Asset assigned to the engagement.
    /// </summary>
    public Guid? AssetId { get; set; }

    /// <summary>Asset on assignment</summary>
    public Asset? Asset { get; set; }
}

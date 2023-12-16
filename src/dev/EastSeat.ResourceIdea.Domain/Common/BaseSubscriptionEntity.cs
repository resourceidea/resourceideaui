namespace EastSeat.ResourceIdea.Domain.Common;

/// <summary>
/// Holds data about the modification and creation of an entity.
/// </summary>
public class BaseSubscriptionEntity
{
    /// <summary>ID of the subscription the entity belongs to.</summary>
    public Guid? SubscriptionId { get; set; } = Guid.Empty;

    /// <summary>Date and time the entity was created.</summary>
    public DateTime Created { get; set; }

    /// <summary>User or agent that created the entity.</summary>
    public string? CreatedBy { get; set; }

    /// <summary>Date and time the entity was last modified.</summary>
    public DateTime? LastModified { get; set; }

    /// <summary>User or agent that last modified the entity.</summary>
    public string? LastModifiedBy { get; set; }

    /// <summary>Flag indicating if the entity is deleted.</summary>
    public bool IsDeleted { get; set; } = false;
}

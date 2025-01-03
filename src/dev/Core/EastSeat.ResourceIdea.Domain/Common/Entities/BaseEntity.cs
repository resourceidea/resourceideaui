namespace EastSeat.ResourceIdea.Domain.Common.Entities;

/// <summary>
/// Common properties for all entities in the system.
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// Tenant DepartmentId for the entity.
    /// </summary>
    public Guid TenantId { get; set; }
    
    /// <summary>
    /// Date when the entity was created.
    /// </summary>
    public DateTimeOffset Created { get; set; }
    
    /// <summary>
    /// User who created the entity.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;
    
    /// <summary>
    /// Date of last modification on the entity.
    /// </summary>
    public DateTimeOffset LastModified { get; set; }
    
    /// <summary>
    /// User who last modified the entity.
    /// </summary>
    public string LastModifiedBy { get; set; } = string.Empty;
    
    /// <summary>
    /// Entity is deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;
    
    /// <summary>
    /// Date when the entity was deleted.
    /// </summary>
    public DateTimeOffset? Deleted { get; set; }

    /// <summary>
    /// User who deleted the entity.
    /// </summary>
    public string DeletedBy { get; set; } = string.Empty;
}
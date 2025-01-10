using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Domain.Common.Entities;

/// <summary>
/// Common properties for all entities in the system.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Tenant DepartmentId for the entity.
    /// </summary>
    public TenantId TenantId { get; set; }
    
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

    /// <summary>
    /// Maps the entity to a model class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model class.</typeparam>
    /// <returns>The model class instance.</returns>
    public abstract TModel ToModel<TModel>();

    public abstract ResourceIdeaResponse<TModel> ToResourceIdeaResponse<TEntity, TModel>()
        where TEntity : BaseEntity
        where TModel : class;
}

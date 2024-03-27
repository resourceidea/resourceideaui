using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EastSeat.ResourceIdea.Persistence.Models;

/// <summary>
/// Base class for all the entities.
/// </summary>
public class BaseEntity
{
    /// <summary>Id of subscription to which the entity belongs to.</summary>
    public Guid SubscriptionId { get; set; }

    /// <summary>Date when the entity was created.</summary>
    public DateTime Created { get; set; }

    /// <summary>Id of the user who created the entity.</summary>
    public string? CreatedBy { get; set; }

    /// <summary>Date when the entity was last modified.</summary>
    public DateTime? LastModified { get; set; }

    /// <summary>Id of the user who last modified the entity.</summary>
    public string? LastModifiedBy { get; set; }

    /// <summary>Flag to indicate if the entity is deleted.</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Entity delete history.</summary>
    public string? DeleteHistory { get; set; }

    /// <summary>Entity modification history.</summary>
    public string? ModificationHistory { get; set; }
}

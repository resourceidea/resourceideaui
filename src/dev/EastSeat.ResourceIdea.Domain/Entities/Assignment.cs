using EastSeat.ResourceIdea.Domain.Common;

namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Engagement assignment.
/// </summary>
public class Assignment : BaseSubscriptionEntity
{
    /// <summary>
    /// Assignment Id.
    /// </summary>
    public Guid Id { get; set; } = Guid.Empty;

    /// <summary>Details of the assignment.</summary>
    public string Details { get; set; } = string.Empty;

    /// <summary>Assignment task name</summary>
    public string Task { get; set; } = string.Empty;

    /// <summary>
    /// Engagement that is being assigned to.
    /// </summary>
    public Guid? EngagementId { get; set; }

    /// <summary>Date when the assignment was started.</summary>
    public DateTime? StartDate { get; set; }

    /// <summary>Date when the assignment was closed.</summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Status of the assignment.
    /// </summary>
    public Constants.Assignment.Status Status { get; set; }

    /// <summary>Engagement where that assignment is added.</summary>
    public Engagement? Engagement { get; set; }

    /// <summary>Subscription to which the assignment belongs.</summary>
    public Subscription? Subscription { get; set; }
}

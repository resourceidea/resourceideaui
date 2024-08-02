using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;

namespace EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;

/// <summary>
/// Represents an engagement task entity.
/// </summary>
public class EngagementTask : BaseEntity
{
    /// <summary>
    /// Gets or sets the ID of the engagement task.
    /// </summary>
    public EngagementTaskId Id { get; set; }

    /// <summary>
    /// Gets or sets the description of the engagement task.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// /// Gets or sets the title of the engagement task.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the status of the engagement task.
    /// </summary>
    public EngagementTaskStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the due date of the engagement task.
    /// </summary>
    public DateTimeOffset DueDate { get; set; }

    /// <summary>
    /// Gets or sets the ID of the engagement associated with the task.
    /// </summary>
    public EngagementId EngagementId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the task is assigned.
    /// </summary>
    public bool IsAssigned { get; set; }

    /// <summary>
    /// Engagement associated with the task.
    /// </summary>
    public Engagement? Engagement { get; set; }

    /// <summary>
    /// Assignments of the engagement task.
    /// </summary>
    public IReadOnlyCollection<EngagementTaskAssignment>? EngagementTaskAssignments { get; set; }
}

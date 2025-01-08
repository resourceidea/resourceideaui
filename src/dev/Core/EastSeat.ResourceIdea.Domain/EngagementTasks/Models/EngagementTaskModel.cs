using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;

namespace EastSeat.ResourceIdea.Domain.EngagementTasks.Models;

public record EngagementTaskModel
{
    /// <summary>
    /// Gets or sets the engagement task ID.
    /// </summary>
    public EngagementTaskId Id { get; init; }

    /// <summary>
    /// Gets or sets the engagement ID.
    /// </summary>
    public EngagementId EngagementId { get; init; }

    /// <summary>
    /// Gets or sets the due date of the engagement task.
    /// </summary>
    public DateTimeOffset? DueDate { get; init; }

    /// <summary>
    /// Gets or sets the description of the engagement task.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets or sets the title of the engagement task.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Gets or sets the status of the engagement task.
    /// </summary>
    public EngagementTaskStatus Status { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the engagement task is assigned.
    /// </summary>
    public bool Assigned { get; init; }
}

using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;

/// <summary>
/// Represents an assignment of an engagement task to an application user.
/// </summary>
public class EngagementTaskAssignment : BaseEntity
{
    /// <summary>
    /// Gets or sets the ID of the engagement task assignment.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the application user assigned to the engagement task.
    /// </summary>
    public ApplicationUserId ApplicationUserId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the engagement task.
    /// </summary>
    public EngagementTaskId EngagementTaskId { get; set; }

    /// <summary>
    /// Gets or sets the start date of the engagement task assignment.
    /// </summary>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date of the engagement task assignment.
    /// /// </summary>
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// Gets or sets the status of the engagement task assignment.
    /// </summary>
    public EngagementTaskAssignmentStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the Engagement task.
    /// </summary>
    public EngagementTask? EngagementTask { get; set; }
}
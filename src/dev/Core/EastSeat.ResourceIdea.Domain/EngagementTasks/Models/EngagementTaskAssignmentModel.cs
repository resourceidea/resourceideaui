using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.EngagementTasks.Models;

/// <summary>
/// Represents an assignment of an engagement task to a user.
/// </summary>
public record EngagementTaskAssignmentModel
{
    /// <summary>
    /// Gets or sets the ID of the engagement task assignment.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets or sets the ID of the application user assigned to the engagement task.
    /// </summary>
    public ApplicationUserId ApplicationUserId { get; init; }

    /// <summary>
    /// Gets or sets the ID of the engagement task.
    /// </summary>
    public EngagementTaskId EngagementTaskId { get; init; }
}
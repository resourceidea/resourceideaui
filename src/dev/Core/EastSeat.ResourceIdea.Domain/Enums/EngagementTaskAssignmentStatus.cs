namespace EastSeat.ResourceIdea.Domain.Enums;

/// <summary>
/// Represents the status of an engagement task assignment.
/// </summary>
public enum EngagementTaskAssignmentStatus
{
    /// <summary>
    /// Active engagement task assignment.
    /// </summary>
    Active,

    /// <summary>
    /// The engagement task assignment has been completed.
    /// </summary>
    Completed,

    /// <summary>
    /// The engagement task assignment has been removed.
    /// </summary>
    Unassigned,
}

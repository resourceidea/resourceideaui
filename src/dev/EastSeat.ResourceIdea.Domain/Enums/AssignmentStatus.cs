namespace EastSeat.ResourceIdea.Domain.Enums;

/// <summary>
/// Assignment statuses
/// </summary>
public enum AssignmentStatus
{
    /// <summary>
    /// Assignment not started.
    /// </summary>
    NotStarted,

    /// <summary>
    /// Assignment is in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// Assignment is closed.
    /// </summary>
    Closed,

    /// <summary>
    /// Assignment is blocked.
    /// </summary>
    Blocked,

    /// <summary>
    /// Assignment has been removed.
    /// </summary>
    Removed
}

namespace EastSeat.ResourceIdea.Domain.Enums;

/// <summary>
/// Represents the status of a task.
/// </summary>
public enum EngagementTaskStatus
{
    /// <summary>
    /// The task has not started yet.
    /// </summary>
    NotStarted,

    /// <summary>
    /// The task is currently in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// The task has been removed.
    /// </summary>
    Removed,

    /// <summary>
    /// The task has been closed.
    /// </summary>
    Closed,

    /// <summary>
    /// The task has been blocked.
    /// </summary>
    Blocked,
}

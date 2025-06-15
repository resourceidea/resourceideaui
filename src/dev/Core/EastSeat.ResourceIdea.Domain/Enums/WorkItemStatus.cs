namespace EastSeat.ResourceIdea.Domain.Enums;

/// <summary>
/// Represents the status of a work item.
/// </summary>
public enum WorkItemStatus
{
    /// <summary>
    /// The work item has been created but not yet started.
    /// </summary>
    NotStarted,

    /// <summary>
    /// The work item is currently in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// The work item is on hold or paused.
    /// </summary>
    OnHold,

    /// <summary>
    /// The work item has been completed.
    /// </summary>
    Completed,

    /// <summary>
    /// The work item has been canceled.
    /// </summary>
    Canceled
}

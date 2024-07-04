namespace EastSeat.ResourceIdea.Domain.Enums;

/// <summary>
/// Represents the status of an engagement.
/// </summary>
public enum EngagementStatus
{
    /// <summary>
    /// The engagement has not started yet.
    /// </summary>
    NotStarted,

    /// <summary>
    /// The engagement is currently in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// The engagement has been canceled.
    /// </summary>
    Canceled,

    /// <summary>
    /// The engagement has been completed.
    /// </summary>
    Completed
}

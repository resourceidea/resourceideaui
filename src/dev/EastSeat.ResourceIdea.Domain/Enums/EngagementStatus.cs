namespace EastSeat.ResourceIdea.Domain.Enums;

/// <summary>
/// Engagement statuses
/// </summary>
public enum EngagementStatus
{
    /// <summary>
    /// Engagement has not started.
    /// </summary>
    NotStarted,

    /// <summary>
    /// Engagement is in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// Engagement has been closed.
    /// </summary>
    Closed,

    /// <summary>
    /// Engagement is blocked.
    /// </summary>
    Blocked
}
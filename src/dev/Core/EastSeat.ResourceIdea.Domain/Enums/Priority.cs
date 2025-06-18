namespace EastSeat.ResourceIdea.Domain.Enums;

/// <summary>
/// Represents the priority levels for work items.
/// </summary>
public enum Priority
{
    /// <summary>
    /// Critical priority - highest urgency.
    /// </summary>
    Critical = 1,

    /// <summary>
    /// High priority.
    /// </summary>
    High = 2,

    /// <summary>
    /// Medium priority - default level.
    /// </summary>
    Medium = 3,

    /// <summary>
    /// Low priority.
    /// </summary>
    Low = 4,

    /// <summary>
    /// Lowest priority.
    /// </summary>
    Lowest = 5
}
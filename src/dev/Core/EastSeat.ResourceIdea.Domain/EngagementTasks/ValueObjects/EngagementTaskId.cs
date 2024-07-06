using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Exceptions;

namespace EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;

/// <summary>
/// Engagement task ID.
/// </summary>
public readonly record struct EngagementTaskId
{
    /// <summary>
    /// Engagement task ID value.
    /// </summary>
    public Guid Value { get; }

    private EngagementTaskId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new Engagement Task ID.
    /// </summary>
    /// <param name="value">Engagement Task ID as a Guid.</param>
    /// <returns>Instance of <see cref="EngagementTaskId"/>.</returns>
    /// <exception cref="InvalidEntityIdException">Thrown when creating a new <see cref="EngagementTaskId"/> from an empty Guid.</exception>
    public static EngagementTaskId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidEntityIdException("EngagementTaskId cannot be empty");
        }

        return new EngagementTaskId(value);
    }

    /// <summary>
    /// Create a new Engagement Task ID.
    /// </summary>
    /// <param name="value">Engagement Task ID as a string.</param>
    /// <returns>Instance of <see cref="EngagementTaskId"/>.</returns>
    /// <exception cref="InvalidEntityIdException">Thrown when creating a new <see cref="EngagementTaskId"/> 
    /// from a string that cannot be parsed to a Guid.</exception>
    public static EngagementTaskId Create(string value)
    {
        if (!Guid.TryParse(value, out var engagementTaskId))
        {
            throw new InvalidEntityIdException("EngagementTaskId is not a valid Guid");
        }

        return Create(engagementTaskId);
    }

    /// <summary>
    /// Empty engagement task id.
    /// </summary>
    public static EngagementTaskId Empty => new(Guid.Empty);
}
using EastSeat.ResourceIdea.Domain.Exceptions;

namespace EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;

/// <summary>
/// Engagement ID.
/// </summary>
public readonly record struct EngagementId
{
    /// <summary>
    /// Client DepartmentId value.
    /// </summary>
    public Guid Value { get; }

    private EngagementId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new Engagement ID.
    /// </summary>
    /// <param name="value">Engagement ID as a Guid.</param>
    /// <returns>Instance of <see cref="EngagementId"/>.</returns>
    /// <exception cref="InvalidEntityIdException">Thrown when creating a new <see cref="EngagementId"/> from an empty Guid.</exception>
    public static EngagementId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidEntityIdException("EngagementId cannot be empty");
        }

        return new EngagementId(value);
    }

    /// <summary>
    /// Create a new Engagement ID.
    /// </summary>
    /// <param name="value">Engagement ID as a string.</param>
    /// <returns>Instance of <see cref="EngagementId"/>.</returns>
    /// <exception cref="InvalidEntityIdException">Thrown when creating a new <see cref="EngagementId"/>
    /// from a string that cannot be parsed to a Guid.</exception>
    public static EngagementId Create(string value)
    {
        if (!Guid.TryParse(value, out var engagementId))
        {
            throw new InvalidEntityIdException("EngagementId is not a valid Guid");
        }

        return Create(engagementId);
    }

    /// <summary>
    /// Empty engagement id.
    /// </summary>
    public static EngagementId Empty => new(Guid.Empty);
}
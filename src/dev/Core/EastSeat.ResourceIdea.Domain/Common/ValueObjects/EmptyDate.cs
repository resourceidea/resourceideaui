namespace EastSeat.ResourceIdea.Domain.Common.ValueObjects;

/// <summary>
/// Represents an empty date.
/// </summary>
public struct EmptyDate
{
    /// <summary>
    /// Gets the value of the empty date.
    /// </summary>
    public static DateTimeOffset Value => DateTimeOffset.MinValue;  // TODO: Set value to meaningful date.

    /// <summary>
    /// Implicit conversion to DateTimeOffset.
    /// </summary>
    /// <param name="emptyDate">The empty date.</param>
    public static implicit operator DateTimeOffset(EmptyDate emptyDate) => Value;
}
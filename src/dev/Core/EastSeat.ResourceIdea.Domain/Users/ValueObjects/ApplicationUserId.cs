using EastSeat.ResourceIdea.Domain.Exceptions;

namespace EastSeat.ResourceIdea.Domain.Users.ValueObjects;

public readonly record struct ApplicationUserId
{
    /// <summary>
    /// Application user id value.
    /// </summary>
    public Guid Value { get; }

    private ApplicationUserId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Create a new Application user id.
    /// </summary>
    /// <param name="value">Application user id as a Guid.</param>
    /// <returns>Instance of <see cref="ApplicationUserId"/>.</returns>
    /// <exception cref="InvalidEntityIdException">Thrown when creating a new ApplicationUserId
    /// from an empty Guid.</exception>
    public static ApplicationUserId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidEntityIdException("ApplicationUserId cannot be empty");
        }

        return new ApplicationUserId(value);
    }

    /// <summary>
    /// Create a new Application user id.
    /// </summary>
    /// <param name="value">Application user ID as a string.</param>
    /// <returns>Instance of <see cref="ApplicationUserId" />.</returns>
    /// <exception cref="InvalidEntityIdException">Thrown when the creating a new ApplicationUserId
    /// from a string value that can't be parsed to a Guid.</exception>
    public static ApplicationUserId Create(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            throw new InvalidEntityIdException("ApplicationUserId is not a valid Guid");
        }

        return Create(guid);
    }

    /// <summary>Application User Id is not empty.</summary>
    public bool IsNotEmpty() => this != Empty;

    /// <summary>
    /// Empty application user id.
    /// </summary>
    public static ApplicationUserId Empty => new(Guid.Empty);
}
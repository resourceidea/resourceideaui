using EastSeat.ResourceIdea.Domain.Exceptions;

namespace EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;

public readonly record struct SubscriptionId
{
    /// <summary>
    /// Subscription id value.
    /// </summary>
    public Guid Value { get; }

    private SubscriptionId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Create a new Subscription Id.
    /// </summary>
    /// <param name="value">Subscription Id as a Guid.</param>
    /// <returns>Instance of <see cref="SubscriptionId"/>.</returns>
    /// <exception cref="InvalidEntityIdException">Thrown when creating a new <see cref="SubscriptionId"/>
    /// from an empty Guid.</exception>
    public static SubscriptionId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidEntityIdException("SubscriptionId cannot be empty");
        }

        return new SubscriptionId(value);
    }

    /// <summary>
    /// Create a new Subscription Id from a string value.
    /// </summary>
    /// <param name="value">Subscription Id as a string value.</param>
    /// <returns>Instance of <see cref="SubscriptionId"/>.</returns>
    /// <exception cref="InvalidEntityIdException">Thrown when creating a new <see cref="SubscriptionId"/>
    /// from a string value that can't be converted to a Guid.</exception>
    public static SubscriptionId Create(string value)
    {
        if (!Guid.TryParse(value, out var subscriptionId))
        {
            throw new InvalidEntityIdException("SubscriptionId is not a valid Guid");
        }

        return Create(subscriptionId);
    }

    /// <summary>
    /// Empty subscription id.
    /// </summary>
    public static SubscriptionId Empty => new(Guid.Empty);
}
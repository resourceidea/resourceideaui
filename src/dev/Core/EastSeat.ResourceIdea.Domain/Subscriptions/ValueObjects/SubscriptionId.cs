using EastSeat.ResourceIdea.Domain.Common.Exceptions;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;

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
    /// Empty subscription id.
    /// </summary>
    public static SubscriptionId Empty => new(Guid.Empty);
}
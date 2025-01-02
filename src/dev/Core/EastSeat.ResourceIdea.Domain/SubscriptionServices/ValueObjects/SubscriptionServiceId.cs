using EastSeat.ResourceIdea.Domain.Exceptions;

namespace EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects
{
    public readonly record struct SubscriptionServiceId
    {
        public Guid Value { get; }

        private SubscriptionServiceId(Guid value)
        {
            Value = value;
        }

        /// <summary>
        /// Create a new subscription service id.
        /// </summary>
        /// <param name="value">Subscription service DepartmentId as a Guid.</param>
        /// <returns>Instance of <see cref="SubscriptionServiceId"/>.</returns>
        /// <exception cref="InvalidEntityIdException">Thrown when creating a new SubscriptionServiceId
        /// from an empty Guid.</exception>
        public static SubscriptionServiceId Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new InvalidEntityIdException("SubscriptionServiceId cannot be empty");
            }

            return new SubscriptionServiceId(value);
        }

        /// <summary>
        /// Create a new subscription service id from a string value.
        /// </summary>
        /// <param name="value">Subscription ID as a string value.</param>
        /// <returns>Instance of <see cref="SubscriptionServiceId"/>.</returns>
        /// <exception cref="InvalidEntityIdException">Thrown when a new SubscriptionServiceId from
        /// a string that cannot be parsed as a Guid.</exception>
        public static SubscriptionServiceId Create(string value)
        {
            if (!Guid.TryParse(value, out var subscriptionServiceId))
            {
                throw new InvalidEntityIdException("SubscriptionServiceId is not a valid Guid");
            }

            return Create(subscriptionServiceId);
        }

        /// <summary>
        /// Create an empty subscription service id.
        /// </summary>
        public static SubscriptionServiceId Empty => new(Guid.Empty);
    }
}
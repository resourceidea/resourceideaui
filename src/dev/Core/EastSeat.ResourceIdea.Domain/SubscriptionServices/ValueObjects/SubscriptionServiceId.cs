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
        /// <param name="value">Subscription service Id as a Guid.</param>
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
        /// Create an empty subscription service id.
        /// </summary>
        public static SubscriptionServiceId Empty => new(Guid.Empty);
    }
}
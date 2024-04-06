using EastSeat.ResourceIdea.Domain.Common.Exceptions;

namespace EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects
{
    public readonly record struct SubscriptionServiceId
    {
        public Guid Value { get; }

        private SubscriptionServiceId(Guid value)
        {
            Value = value;
        }

        public static SubscriptionServiceId Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new InvalidEntityIdException("SubscriptionServiceId cannot be empty");
            }

            return new SubscriptionServiceId(value);
        }
    }
}
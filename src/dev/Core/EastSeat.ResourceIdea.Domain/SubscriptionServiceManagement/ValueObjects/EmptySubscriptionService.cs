namespace EastSeat.ResourceIdea.Domain.SubscriptionServiceManagement.ValueObjects;

public sealed record EmptySubscriptionService
{
    public static Entities.SubscriptionService Instance { get; } = new()
    {
        Id = SubscriptionServiceId.Create(Guid.Empty),
        Name = string.Empty
    };

    private EmptySubscriptionService() { }
}

using EastSeat.ResourceIdea.Domain.Constants;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscription.Commands.CreateSubscription;

/// <summary>
/// Command to create a new subscription.
/// </summary>
public class CreateSubscriptionCommand : IRequest<CreateSubscriptionCommandResponse>
{
    /// <summary>Subscription ID.</summary>
    public Guid SubscriptionId { get; set; } = Guid.NewGuid();

    /// <summary>Subscriber's name.</summary>
    public string? SubscriberName { get; set; }

    /// <summary>Subscription start date.</summary>
    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    /// <summary>Subscription status.</summary>
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
}

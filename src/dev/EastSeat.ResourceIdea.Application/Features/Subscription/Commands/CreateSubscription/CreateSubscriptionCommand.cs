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

    /// <summary>Application user's first name.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Application user's last name.</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>Application user's email.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Application user's password.</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>Subscription start date.</summary>
    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    /// <summary>Employee's job position id.</summary>
    public Guid JobPositionId { get; set; } = Guid.Empty;

    /// <summary>Subscription status.</summary>
    public Constants.Subscription.Status Status { get; set; } = Constants.Subscription.Status.Active;
}

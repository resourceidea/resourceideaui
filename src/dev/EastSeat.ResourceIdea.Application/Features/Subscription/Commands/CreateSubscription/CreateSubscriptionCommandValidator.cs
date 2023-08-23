using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.Subscription.Commands.CreateSubscription;

/// <summary>
/// Validates the command to create a subscription.
/// </summary>
public class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
{
    public CreateSubscriptionCommandValidator()
    {
        RuleFor(subscription => subscription.SubscriberName)
            .NotEmpty().WithMessage("Subscriber's name is required.")
            .NotNull();

        RuleFor(subscription => subscription.SubscriptionId)
            .NotEqual(Guid.Empty).WithMessage("Subscription ID is required.");

        RuleFor(subscription => subscription.StartDate)
            .NotEmpty().WithMessage("Subscription start date is required.")
            .NotNull();
    }
}

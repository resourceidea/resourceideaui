using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.Subscription.Commands.CreateSubscription;

/// <summary>
/// Validates the command to create a subscription.
/// </summary>
public class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
{
    public CreateSubscriptionCommandValidator()
    {
        RuleFor(subscription => subscription.FirstName)
            .NotEmpty().WithMessage(Constants.ErrorCodes.Validators.CreateSubscription.MissingFirstName)
            .NotNull();

        RuleFor(subscription => subscription.LastName)
            .NotEmpty().WithMessage(Constants.ErrorCodes.Validators.CreateSubscription.MissingLastName)
            .NotNull();

        RuleFor(subscription => subscription.Email)
            .NotEmpty().WithMessage(Constants.ErrorCodes.Validators.CreateSubscription.MissingEmail)
            .NotNull();

        RuleFor(subscription => subscription.Password)
            .NotEmpty().WithMessage(Constants.ErrorCodes.Validators.CreateSubscription.MissingPassword)
            .NotNull();

        RuleFor(subscription => subscription.SubscriberName)
            .NotEmpty().WithMessage(Constants.ErrorCodes.Validators.CreateSubscription.MissingSubscriberName)
            .NotNull();

        RuleFor(subscription => subscription.SubscriptionId)
            .NotEqual(Guid.Empty).WithMessage(Constants.ErrorCodes.Validators.CreateSubscription.MissingSubscriptionId);

        RuleFor(subscription => subscription.StartDate)
            .NotEmpty().WithMessage(Constants.ErrorCodes.Validators.CreateSubscription.MissingSubscriptionStartDate)
            .NotNull();
    }
}

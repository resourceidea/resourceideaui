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
            .NotEmpty().WithMessage(Constants.ErrorCodes.MissingFirstName)
            .NotNull();

        RuleFor(subscription => subscription.LastName)
            .NotEmpty().WithMessage(Constants.ErrorCodes.MissingLastName)
            .NotNull();

        RuleFor(subscription => subscription.Email)
            .NotEmpty().WithMessage(Constants.ErrorCodes.MissingEmail)
            .NotNull();

        RuleFor(subscription => subscription.Password)
            .NotEmpty().WithMessage(Constants.ErrorCodes.MissingPassword)
            .NotNull();

        RuleFor(subscription => subscription.SubscriberName)
            .NotEmpty().WithMessage(Constants.ErrorCodes.MissingSubscriberName)
            .NotNull();

        RuleFor(subscription => subscription.SubscriptionId)
            .NotEqual(Guid.Empty).WithMessage(Constants.ErrorCodes.MissingSubscriptionId);

        RuleFor(subscription => subscription.StartDate)
            .NotEmpty().WithMessage(Constants.ErrorCodes.MissingSubscriptionStartDate)
            .NotNull();
    }
}

using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Commands;

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Validators;

public sealed class UpdateSubscriptionServiceCommandValidator : AbstractValidator<UpdateSubscriptionServiceCommand>
{
    public UpdateSubscriptionServiceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Service name is required.");
    }
}
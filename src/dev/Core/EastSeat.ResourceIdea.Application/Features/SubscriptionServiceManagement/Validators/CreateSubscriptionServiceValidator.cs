using EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Commands;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Validators;

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Validators;

/// <summary>
/// Validates the command to create a service that can be subscribed to by the tenants.
/// </summary>
public sealed class CreateSubscriptionServiceValidator : AbstractValidator<CreateSubscriptionServiceCommand>
{
    public CreateSubscriptionServiceValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Service name is required.");
    }
}
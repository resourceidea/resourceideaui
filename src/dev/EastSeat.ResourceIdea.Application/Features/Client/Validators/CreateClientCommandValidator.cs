using EastSeat.ResourceIdea.Application.Features.Client.Commands;
using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.Client.Validators;

class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    private const int ColorCodeLength = 6;

    public CreateClientCommandValidator()
    {
        RuleFor(client => client.Name)
            .NotEmpty().WithMessage("Client name is required.")
            .NotNull();

        RuleFor(client => client.ColorCode ?? string.Empty)
            .Must(BeColorCodeOfValidLength)
            .Matches(@"^[a-fA-F0-9]*$")
            .WithMessage("Invalid client color code.");

        RuleFor(client => client.SubscriptionId)
            .Must(BeANonEmptySubscriptionId).WithMessage("Empty Subscription ID is not allowed.")
            .NotNull();
    }

    private bool BeColorCodeOfValidLength(string colorCode)
    {
        return !string.IsNullOrEmpty(colorCode) && colorCode.Length == ColorCodeLength;
    }

    private bool BeANonEmptySubscriptionId(Guid subscriptionId)
    {
        return subscriptionId != Guid.Empty;
    }
}

using EastSeat.ResourceIdea.Application.Features.Client.Commands;
using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.Client.Validators;

public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    private const int ColorCodeLength = 6;

    public CreateClientCommandValidator()
    {
        RuleFor(client => client.Name)
            .NotEmpty().WithMessage("Client name is required.")
            .NotNull();

        RuleFor(client => client.ColorCode)
            .Must(x => string.IsNullOrEmpty(x) || x.Length == ColorCodeLength)
            .WithMessage("Invalid client color code.");

        RuleFor(client => client.SubscriptionId)
            .NotEmpty().WithMessage("Subscription ID is required.")
            .NotNull();
    }
}

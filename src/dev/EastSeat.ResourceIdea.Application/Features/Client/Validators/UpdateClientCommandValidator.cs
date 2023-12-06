using EastSeat.ResourceIdea.Application.Features.Client.Commands;

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.Client.Validators;

public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    private const int ColorCodeLength = 6;

    public UpdateClientCommandValidator()
    {
        RuleFor(client => client.Id)
            .NotEmpty().WithMessage("Client ID is required.")
            .NotNull();

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

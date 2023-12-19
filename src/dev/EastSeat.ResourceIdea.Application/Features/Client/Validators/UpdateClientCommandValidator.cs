using EastSeat.ResourceIdea.Application.Features.Client.Commands;

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.Client.Validators;

public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    private const int ColorCodeLength = 6;

    public UpdateClientCommandValidator()
    {
        RuleFor(client => client.Id)
            .Must(BeANonEmptyGuid)
            .WithMessage("Client ID is required.");

        RuleFor(client => client.Name)
            .NotEmpty().WithMessage("Client name is required.")
            .NotNull();

        RuleFor(client => client.ColorCode)
            .Must(BeAValidColorCode)
            .WithMessage("Invalid client color code.");

        RuleFor(client => client.SubscriptionId)
            .Must(BeANonEmptyGuid)
            .WithMessage("Subscription ID is required.");
    }

    private bool BeAValidColorCode(string colorCode)
    {
        return !string.IsNullOrEmpty(colorCode) && colorCode.Length == ColorCodeLength;
    }

    private bool BeANonEmptyGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}

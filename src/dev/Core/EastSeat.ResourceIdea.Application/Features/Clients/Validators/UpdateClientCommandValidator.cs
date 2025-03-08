// ----------------------------------------------------------------------------------
// File: UpdateClientCommandValidator.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Clients\Validators\UpdateClientCommandValidator.cs
// Description: Validator for UpdateClientCommand.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Validators;

public sealed class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("ClientId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Address)
            .Must(BeValidAddress).WithMessage("Client address must be valid.");
    }

    private bool BeValidAddress(Address address) => address.IsNotEmpty();
}
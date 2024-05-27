namespace EastSeat.ResourceIdea.Application.Features.Clients.Validators
{
    using FluentValidation;

    using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
    using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
    using System;

    /// <summary>
    /// Validates the command to create a client.
    /// </summary>
    public sealed class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Client name is required.");
            
            RuleFor(x => x.Address)
                .Must(BeValidAddress)
                .WithMessage("Client address is must be a valid address.");
        }

        private bool BeValidAddress(Address address) => address.IsNotEmpty();
    }
}
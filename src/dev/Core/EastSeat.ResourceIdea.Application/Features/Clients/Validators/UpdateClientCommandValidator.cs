using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
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

        RuleFor(x => x.TenantId)
            .Must(BeValidTenantId).WithMessage("Client Tenant Id must be valid.");

        RuleFor(x => x.Address)
            .Must(BeValidAddress).WithMessage("Client address must be valid.");
    }

    private bool BeValidTenantId(TenantId id) => id.IsNotEmpty();

    private bool BeValidAddress(Address address) => address.IsNotEmpty();
}
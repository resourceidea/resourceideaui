using EastSeat.ResourceIdea.Application.Features.Tenants.Commands;

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Validators;

public sealed class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
{
    public UpdateTenantCommandValidator()
    {
        RuleFor(x => x.Organization)
            .NotEmpty().WithMessage("Organization is required.")
            .MaximumLength(200).WithMessage("Organization must not exceed 200 characters.");
    }
}

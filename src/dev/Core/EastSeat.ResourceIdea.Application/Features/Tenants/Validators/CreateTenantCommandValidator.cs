using EastSeat.ResourceIdea.Application.Features.Tenants.Commands;

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Validators;

/// <summary>
/// Validates <see cref="CreateTenantCommand"/>.
/// </summary>
public sealed class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.Organization)
            .NotEmpty().WithMessage("Organization is required.")
            .MaximumLength(200).WithMessage("Organization must not exceed 200 characters.");
    }
}
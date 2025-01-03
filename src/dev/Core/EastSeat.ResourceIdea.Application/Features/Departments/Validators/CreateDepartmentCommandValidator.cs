using EastSeat.ResourceIdea.Application.Features.Departments.Commands;

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.Departments.Validators;

/// <summary>
/// Validates the command to create a department.
/// </summary>
public sealed class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Department name is required.")
            .MaximumLength(250).WithMessage("Department name must not exceed 250 characters.");
    }
}

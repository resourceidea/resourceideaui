using FluentValidation;
using EastSeat.ResourceIdea.Application.Features.Departments.Commands;

namespace EastSeat.ResourceIdea.Application.Features.Departments.Validators;

/// <summary>
/// Validator for the <see cref="UpdateDepartmentCommand"/>.
/// </summary>
public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotNull().WithMessage("DepartmentId is required.");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
    }
}

using FluentValidation;

namespace EastSeat.ResourceIdea.Application.Features.Employee.Commands.CreateEmployee;

/// <summary>
/// Validates the command to create an employee.
/// </summary>
public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(employee => employee.UserId)
            .NotEmpty().WithMessage(Constants.ErrorCodes.MissingUserId)
            .NotNull();

        RuleFor(employee => employee.SubscriptionId)
            .NotEqual(Guid.Empty).WithMessage(Constants.ErrorCodes.MissingSubscriptionId);

        RuleFor(employee => employee.JobPositionId)
            .NotEmpty().WithMessage(Constants.ErrorCodes.MissingJobPositionId)
            .NotNull();
    }
}

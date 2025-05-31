// =========================================================================================
// File: UpdateEmployeeCommandHandler.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Handlers\UpdateEmployeeCommandHandler.cs
// Description: Handles the command to update an employee's details.
// =========================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Employees.Commands;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Handlers;

public class UpdateEmployeeCommandHandler(IEmployeeService employeeService)
    : BaseHandler, IRequestHandler<UpdateEmployeeCommand, ResourceIdeaResponse<EmployeeModel>>
{
    private readonly IEmployeeService _employeeService = employeeService;

    public async Task<ResourceIdeaResponse<EmployeeModel>> Handle(
        UpdateEmployeeCommand command,
        CancellationToken cancellationToken)
    {
        ValidationResponse commandValidation = command.Validate();
        if (!commandValidation.IsValid && commandValidation.ValidationFailureMessages.Any())
        {
            // TODO: Log validation failure.
            return ResourceIdeaResponse<EmployeeModel>.Failure(ErrorCode.CommandValidationFailure);
        }

        Employee employee = command.ToEntity();
        var updateEmployeeResponse = await _employeeService.UpdateAsync(employee, cancellationToken);
        if (updateEmployeeResponse.IsFailure)
        {
            return ResourceIdeaResponse<EmployeeModel>.Failure(updateEmployeeResponse.Error);
        }

        return updateEmployeeResponse.Content.ToResourceIdeaResponse<Employee, EmployeeModel>();
    }
}

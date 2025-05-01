// ================================================================================================
// File: AddEmployeeCommandHandler.cs
// Path: src/dev/EastSeat.ResourceIdea.Application/Features/Employees/Handlers/AddEmployeeCommandHandler.cs
// Description: Handles the command to add an employee.
// ================================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Employees.Commands;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Handlers;

public class AddEmployeeCommandHandler(IEmployeeService employeeService)
    : BaseHandler, IRequestHandler<AddEmployeeCommand, ResourceIdeaResponse<EmployeeModel>>
{
    private readonly IEmployeeService _employeeService = employeeService;

    public async Task<ResourceIdeaResponse<EmployeeModel>> Handle(
        AddEmployeeCommand command,
        CancellationToken cancellationToken)
    {
        ValidationResponse commandValidation = command.Validate();
        if (!commandValidation.IsValid && commandValidation.ValidationFailureMessages.Any())
        {
            // TODO: Log validation failure.
            return ResourceIdeaResponse<EmployeeModel>.Failure(ErrorCode.CommandValidationFailure);
        }

        Employee newEmployee = command.ToEntity();
        var addEmployeeResponse = await _employeeService.AddAsync(newEmployee, cancellationToken);
        if (addEmployeeResponse.IsFailure)
        {
            // TODO: Log failure to add an employee.
            return ResourceIdeaResponse<EmployeeModel>.Failure(addEmployeeResponse.Error);
        }

        return addEmployeeResponse.Content.Value.ToResourceIdeaResponse<Employee, EmployeeModel>();
    }
}

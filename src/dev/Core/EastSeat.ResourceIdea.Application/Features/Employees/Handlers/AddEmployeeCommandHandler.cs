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
using EastSeat.ResourceIdea.Domain.Users.Entities;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Handlers;

public class AddEmployeeCommandHandler(
    IEmployeeService employeeService,
    IApplicationUserService applicationUserService)
    : BaseHandler, IRequestHandler<AddEmployeeCommand, ResourceIdeaResponse<EmployeeModel>>
{
    private readonly IEmployeeService _employeeService = employeeService;
    private readonly IApplicationUserService _applicationUserService = applicationUserService;

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

        var addApplicationUserResponse = await _applicationUserService.AddApplicationUserAsync(
            command.FirstName,
            command.LastName,
            command.Email,
            command.TenantId
        );
        if (addApplicationUserResponse.IsFailure)
        {
            // TODO: Log failure to add an application user.
            return ResourceIdeaResponse<EmployeeModel>.Failure(addApplicationUserResponse.Error);
        }

        IApplicationUser newApplicationUser = addApplicationUserResponse.Content.Value;
        Employee newEmployee = command.ToEntity();
        newEmployee.ApplicationUserId = newApplicationUser.ApplicationUserId;
        var addEmployeeResponse = await _employeeService.AddAsync(newEmployee, cancellationToken);
        if (addEmployeeResponse.IsFailure)
        {
            await _applicationUserService.DeleteApplicationUserAsync(newApplicationUser.ApplicationUserId);
            
            // TODO: Log failure to add an employee.
            return ResourceIdeaResponse<EmployeeModel>.Failure(addEmployeeResponse.Error);
        }

        return addEmployeeResponse.Content.Value.ToResourceIdeaResponse<Employee, EmployeeModel>();
    }
}

// ===================================================================================
// File: GetEmployeeByApplicationUserIdQueryHandler.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Handlers\GetEmployeeByApplicationUserIdQueryHandler.cs
// Description: Handles the query to get an employee by ApplicationUserId.
// ===================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Application.Features.Employees.Queries;
using EastSeat.ResourceIdea.Application.Features.Employees.Specifications;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Handlers;

public sealed class GetEmployeeByApplicationUserIdQueryHandler(IEmployeeService employeeService)
    : BaseHandler, IRequestHandler<GetEmployeeByApplicationUserIdQuery, ResourceIdeaResponse<EmployeeModel>>
{
    private readonly IEmployeeService _employeeService = employeeService;

    public async Task<ResourceIdeaResponse<EmployeeModel>> Handle(
        GetEmployeeByApplicationUserIdQuery query,
        CancellationToken cancellationToken)
    {
        ValidationResponse queryValidation = query.Validate();
        if (!queryValidation.IsValid && queryValidation.ValidationFailureMessages.Any())
        {
            return ResourceIdeaResponse<EmployeeModel>.Failure(ErrorCode.EmployeeQueryValidationFailure);
        }

        BaseSpecification<Employee> specification = new GetEmployeeByApplicationUserIdSpecification(query.ApplicationUserId, query.TenantId);
        ResourceIdeaResponse<Employee> employeeQuery = await _employeeService.GetByIdAsync(specification, cancellationToken);
        if (employeeQuery.IsFailure)
        {
            return ResourceIdeaResponse<EmployeeModel>.Failure(employeeQuery.Error);
        }

        if (!employeeQuery.Content.HasValue)
        {
            return ResourceIdeaResponse<EmployeeModel>.Failure(ErrorCode.EmployeeNotFound);
        }

        return employeeQuery.Content.Value.ToResourceIdeaResponse<Employee, EmployeeModel>();
    }
}
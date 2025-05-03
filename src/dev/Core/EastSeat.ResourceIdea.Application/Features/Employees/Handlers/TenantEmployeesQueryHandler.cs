// ================================================================================
// File: TenantEmployeesQueryHandler.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\JobPositions\Handlers\TenantEmployeesQueryHandler.cs
// Description: This file contains the definition of the TenantEmployeesQueryHandler class.
// ================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Application.Features.Employees.Queries;
using EastSeat.ResourceIdea.Application.Features.Employees.Specifications;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Handlers;

/// <summary>
/// Handles the query for tenant's employees.
/// </summary>
/// <param name="employeeService">Service responsible for employee handling operations.</param>
public class TenantEmployeesQueryHandler(IEmployeeService employeeService)
    : BaseHandler,
      IRequestHandler<TenantEmployeesQuery, ResourceIdeaResponse<PagedListResponse<TenantEmployeeModel>>>
{
    private readonly IEmployeeService _employeeService = employeeService;

    public async Task<ResourceIdeaResponse<PagedListResponse<TenantEmployeeModel>>> Handle(
        TenantEmployeesQuery query,
        CancellationToken cancellationToken)
    {
        var querySpecification = new TenantEmployeesSpecification(query.TenantId);
        var queryResponse  = await _employeeService.GetPagedListAsync(
            query.PageNumber,
            query.PageSize,
            querySpecification,
            cancellationToken);

        return queryResponse.ToResourceIdeaResponse<Employee, TenantEmployeeModel>();
    }
}

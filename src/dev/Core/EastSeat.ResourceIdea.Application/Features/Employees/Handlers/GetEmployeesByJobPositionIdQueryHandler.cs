// ================================================================================
// File: GetEmployeesByJobPositionIdQueryHandler.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Handlers\GetEmployeesByJobPositionIdQueryHandler.cs
// Description: This file contains the definition of the GetEmployeesByJobPositionIdQueryHandler class.
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
using System.Diagnostics;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Handlers;

/// <summary>
/// Handles the query for getting employees by job position ID.
/// </summary>
/// <param name="employeeService">Service responsible for employee handling operations.</param>
public class GetEmployeesByJobPositionIdQueryHandler(IEmployeeService employeeService)
    : BaseHandler,
      IRequestHandler<GetEmployeesByJobPositionIdQuery, ResourceIdeaResponse<PagedListResponse<TenantEmployeeModel>>>
{
    private readonly IEmployeeService _employeeService = employeeService;
    private static readonly ActivitySource ActivitySource = new("EastSeat.ResourceIdea.Application");

    public async Task<ResourceIdeaResponse<PagedListResponse<TenantEmployeeModel>>> Handle(
        GetEmployeesByJobPositionIdQuery query,
        CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("GetEmployeesByJobPositionIdQuery.Handle");
        activity?.SetTag("tenant.id", query.TenantId.ToString());
        activity?.SetTag("jobPosition.id", query.JobPositionId.ToString());
        activity?.SetTag("query.pageNumber", query.PageNumber);
        activity?.SetTag("query.pageSize", query.PageSize);

        var querySpecification = new EmployeesByJobPositionSpecification(query.TenantId, query.JobPositionId);
        var queryResponse = await _employeeService.GetPagedListAsync(
            query.PageNumber,
            query.PageSize,
            querySpecification,
            cancellationToken);

        if (queryResponse.IsSuccess && queryResponse.Content.HasValue)
        {
            activity?.SetTag("result.totalCount", queryResponse.Content.Value.TotalCount);
            activity?.SetTag("result.itemCount", queryResponse.Content.Value.Items.Count);
        }

        activity?.SetTag("operation.result", queryResponse.IsSuccess ? "success" : "failure");

        return queryResponse.ToResourceIdeaResponse<Employee, TenantEmployeeModel>();
    }
}
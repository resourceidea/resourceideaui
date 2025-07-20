// ================================================================================
// File: GetPotentialManagersQueryHandler.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Handlers\GetPotentialManagersQueryHandler.cs
// Description: Handles the query to get potential managers for an employee.
// ================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Application.Features.Employees.Queries;
using EastSeat.ResourceIdea.Application.Features.Employees.Specifications;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using MediatR;
using System.Diagnostics;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Handlers;

/// <summary>
/// Handles the query to get potential managers for an employee.
/// </summary>
/// <param name="employeeService">Service responsible for employee handling operations.</param>
public class GetPotentialManagersQueryHandler(IEmployeeService employeeService)
    : BaseHandler,
      IRequestHandler<GetPotentialManagersQuery, ResourceIdeaResponse<PagedListResponse<TenantEmployeeModel>>>
{
    private readonly IEmployeeService _employeeService = employeeService;
    private static readonly ActivitySource ActivitySource = new("EastSeat.ResourceIdea.Application");

    public async Task<ResourceIdeaResponse<PagedListResponse<TenantEmployeeModel>>> Handle(
        GetPotentialManagersQuery query,
        CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("GetPotentialManagersQuery.Handle");
        activity?.SetTag("tenant.id", query.TenantId.ToString());
        activity?.SetTag("exclude.employee.id", query.ExcludeEmployeeId.ToString());
        activity?.SetTag("query.pageNumber", query.PageNumber);
        activity?.SetTag("query.pageSize", query.PageSize);

        var queryValidation = query.Validate();
        if (!queryValidation.IsValid && queryValidation.ValidationFailureMessages.Any())
        {
            activity?.SetTag("operation.result", "validation_failure");
            return ResourceIdeaResponse<PagedListResponse<TenantEmployeeModel>>.Failure(ErrorCode.EmployeeQueryValidationFailure);
        }

        var querySpecification = new PotentialManagersSpecification(query.TenantId, query.ExcludeEmployeeId);
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
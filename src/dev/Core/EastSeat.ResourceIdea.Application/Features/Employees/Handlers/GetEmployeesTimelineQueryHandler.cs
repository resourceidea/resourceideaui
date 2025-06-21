// ===========================================================================
// File: GetEmployeesTimelineQueryHandler.cs
// Path: src/dev/Core/EastSeat.ResourceIdea.Application/Features/Employees/Handlers/GetEmployeesTimelineQueryHandler.cs
// Description: Handler for the GetEmployeesTimelineQuery.
// ===========================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Application.Features.Employees.Queries;
using EastSeat.ResourceIdea.Application.Features.Employees.Specifications;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Contracts;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Specifications;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.WorkItems.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Handlers;

/// <summary>
/// Handles the GetEmployeesTimelineQuery.
/// </summary>
/// <param name="employeeService">Service responsible for employee operations.</param>
/// <param name="workItemsService">Service responsible for work items operations.</param>
public class GetEmployeesTimelineQueryHandler(IEmployeeService employeeService, IWorkItemsService workItemsService)
    : BaseHandler,
      IRequestHandler<GetEmployeesTimelineQuery, ResourceIdeaResponse<List<EmployeeTimelineModel>>>
{
    private readonly IEmployeeService _employeeService = employeeService;
    private readonly IWorkItemsService _workItemsService = workItemsService;

    public async Task<ResourceIdeaResponse<List<EmployeeTimelineModel>>> Handle(
        GetEmployeesTimelineQuery request,
        CancellationToken cancellationToken)
    {
        // Get all employees for the tenant
        var employeesSpecification = new TenantEmployeesSpecification(request.TenantId);
        var employeesResult = await _employeeService.GetPagedListAsync(
            1,
            1000, // Get a large number to get all employees
            employeesSpecification,
            cancellationToken);

        if (employeesResult.IsFailure)
        {
            return ResourceIdeaResponse<List<EmployeeTimelineModel>>.Failure(employeesResult.Error);
        }

        if (!employeesResult.Content.HasValue)
        {
            return ResourceIdeaResponse<List<EmployeeTimelineModel>>.Success(new List<EmployeeTimelineModel>());
        }

        // Convert the result to get TenantEmployeeModel collection
        var employeesResponse = employeesResult.ToResourceIdeaResponse<Employee, TenantEmployeeModel>();
        if (!employeesResponse.IsSuccess || !employeesResponse.Content.HasValue)
        {
            return ResourceIdeaResponse<List<EmployeeTimelineModel>>.Success(new List<EmployeeTimelineModel>());
        }

        var employees = employeesResponse.Content.Value.Items;

        // Filter employees by search term if provided
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLowerInvariant();
            employees = employees.Where(e => 
                e.FirstName?.ToLowerInvariant().Contains(searchTerm) == true ||
                e.LastName?.ToLowerInvariant().Contains(searchTerm) == true ||
                e.Email?.ToLowerInvariant().Contains(searchTerm) == true ||
                e.JobPositionTitle?.ToLowerInvariant().Contains(searchTerm) == true ||
                e.DepartmentName?.ToLowerInvariant().Contains(searchTerm) == true
            ).ToList();
        }

        // Get work items for the date range
        var workItemsSpecification = new TenantWorkItemsSpecification(request.TenantId);
        var workItemsResult = await _workItemsService.GetPagedListAsync(
            1,
            10000, // Get a large number to get all work items
            workItemsSpecification,
            cancellationToken);

        var workItems = new List<WorkItemModel>();
        if (workItemsResult.IsSuccess && workItemsResult.Content.HasValue)
        {
            // Convert work items result to models
            var workItemsResponse = workItemsResult.ToResourceIdeaResponse<WorkItem, WorkItemModel>();
            if (workItemsResponse.IsSuccess && workItemsResponse.Content.HasValue)
            {
                workItems = workItemsResponse.Content.Value.Items.Where(wi => 
                    wi.StartDate.HasValue && 
                    wi.StartDate.Value.Date >= request.StartDate.ToDateTime(TimeOnly.MinValue) &&
                    wi.StartDate.Value.Date <= request.EndDate.ToDateTime(TimeOnly.MaxValue))
                .ToList();
            }
        }

        // Create the timeline models
        var timelineModels = employees.Select(employee => new EmployeeTimelineModel
        {
            EmployeeId = employee.EmployeeId,
            FirstName = employee.FirstName ?? string.Empty,
            LastName = employee.LastName ?? string.Empty,
            Email = employee.Email ?? string.Empty,
            JobPositionTitle = employee.JobPositionTitle ?? string.Empty,
            DepartmentName = employee.DepartmentName ?? string.Empty,
            WorkItems = workItems.Where(wi => wi.AssignedToId == employee.EmployeeId).ToList()
        }).ToList();

        return ResourceIdeaResponse<List<EmployeeTimelineModel>>.Success(timelineModels);
    }
}
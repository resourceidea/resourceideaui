// ----------------------------------------------------------------------------------
// File: WorkItemMapper.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Mappers\WorkItemMapper.cs
// Description: Provides extension methods for mapping WorkItem entities to models.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.WorkItems.Models;

namespace EastSeat.ResourceIdea.Application.Mappers;

/// <summary>
/// Provides extension methods for mapping WorkItem entities to models.
/// </summary>
public static class WorkItemMapper
{
    /// <summary>
    /// Maps a WorkItem entity to a model of type TModel.
    /// </summary>
    /// <typeparam name="TModel">The type of the model to map to.</typeparam>
    /// <param name="workItem">The work item entity to map.</param>
    /// <returns>The mapped model of type TModel.</returns>
    /// <exception cref="InvalidCastException">Thrown when the cast to TModel fails.</exception>
    /// <exception cref="NotSupportedException">Thrown when the mapping to the specified type is not supported.</exception>
    public static TModel ToModel<TModel>(this WorkItem workItem) where TModel : class
    {
        return workItem switch
        {
            WorkItem w when typeof(TModel) == typeof(WorkItemModel) => ToWorkItemModel(w) as TModel ?? throw new InvalidCastException(),
            _ => throw new NotSupportedException($"Mapping of type {typeof(TModel).Name} is not supported")
        };
    }

    /// <summary>
    /// Maps a WorkItemModel to a WorkItem entity.
    /// </summary>
    /// <param name="model">The work item model to map.</param>
    /// <returns>The mapped WorkItem entity.</returns>
    public static WorkItem ToEntity(this WorkItemModel model)
    {
        return new WorkItem
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            EngagementId = model.EngagementId,
            TenantId = model.TenantId,
            PlannedStartDate = model.StartDate,
            CompletedDate = model.CompletedDate,
            Status = model.Status,
            Priority = model.Priority,
            AssignedToId = model.AssignedToId
        };
    }

    /// <summary>
    /// Maps a WorkItem entity to a ResourceIdeaResponse of WorkItemModel.
    /// </summary>
    /// <param name="workItem">The work item entity to map.</param>
    /// <returns>The mapped ResourceIdeaResponse of WorkItemModel.</returns>
    public static ResourceIdeaResponse<WorkItemModel> ToResourceIdeaResponse(this WorkItem workItem)
    {
        return ResourceIdeaResponse<WorkItemModel>.Success(ToWorkItemModel(workItem));
    }

    /// <summary>
    /// Maps a PagedListResponse of WorkItem entities to a ResourceIdeaResponse of PagedListResponse of WorkItemModel.
    /// </summary>
    /// <param name="workItems">The paged list of work item entities to map.</param>
    /// <returns>The mapped ResourceIdeaResponse of PagedListResponse of WorkItemModel.</returns>
    public static ResourceIdeaResponse<PagedListResponse<WorkItemModel>> ToResourceIdeaResponse(this PagedListResponse<WorkItem> workItems)
    {
        return ResourceIdeaResponse<PagedListResponse<WorkItemModel>>.Success(ToModelPagedListResponse(workItems));
    }

    /// <summary>
    /// Maps a WorkItem entity to a WorkItemModel.
    /// </summary>
    /// <param name="workItem">The work item entity to map.</param>
    /// <returns>The mapped WorkItemModel.</returns>
    private static WorkItemModel ToWorkItemModel(WorkItem workItem)
    {
        return new WorkItemModel
        {
            Id = workItem.Id,
            Title = workItem.Title,
            Description = workItem.Description ?? string.Empty,
            EngagementId = workItem.EngagementId,
            EngagementTitle = workItem.Engagement?.Title ?? string.Empty,
            TenantId = workItem.TenantId,
            StartDate = workItem.PlannedStartDate,
            CompletedDate = workItem.CompletedDate,
            Status = workItem.Status,
            Priority = workItem.Priority,
            AssignedToId = workItem.AssignedToId,
            AssignedToName = GetEmployeeName(workItem.AssignedTo)
        };
    }

    /// <summary>
    /// Maps a PagedListResponse of WorkItem entities to a PagedListResponse of WorkItemModel.
    /// </summary>
    /// <param name="workItems">The paged list of work item entities to map.</param>
    /// <returns>The mapped PagedListResponse of WorkItemModel.</returns>
    private static PagedListResponse<WorkItemModel> ToModelPagedListResponse(PagedListResponse<WorkItem> workItems)
    {
        return new()
        {
            Items = [.. workItems.Items.Select(ToWorkItemModel)],
            CurrentPage = workItems.CurrentPage,
            PageSize = workItems.PageSize,
            TotalCount = workItems.TotalCount
        };
    }

    /// <summary>
    /// Helper method to get the full name of an employee.
    /// </summary>
    /// <param name="employee">The employee entity.</param>
    /// <returns>The full name of the employee or empty string if null.</returns>
    private static string GetEmployeeName(Domain.Employees.Entities.Employee? employee)
    {
        if (employee == null)
            return string.Empty;

        var firstName = employee.FirstName?.Trim() ?? string.Empty;
        var lastName = employee.LastName?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            return string.Empty;

        if (string.IsNullOrEmpty(firstName))
            return lastName;

        if (string.IsNullOrEmpty(lastName))
            return firstName;

        return $"{firstName} {lastName}";
    }

    /// <summary>
    /// Maps an UpdateWorkItemCommand to a WorkItem entity.
    /// </summary>
    /// <param name="command">The update work item command to map.</param>
    /// <returns>The mapped WorkItem entity.</returns>
    public static WorkItem ToEntity(this Application.Features.WorkItems.Commands.UpdateWorkItemCommand command)
    {
        return new WorkItem
        {
            Id = command.WorkItemId,
            Title = command.Title,
            Description = command.Description,
            EngagementId = command.EngagementId,
            TenantId = command.TenantId,
            PlannedStartDate = command.StartDate,
            CompletedDate = command.CompletedDate,
            Status = command.Status,
            Priority = command.Priority,
            AssignedToId = command.AssignedToId
        };
    }
}

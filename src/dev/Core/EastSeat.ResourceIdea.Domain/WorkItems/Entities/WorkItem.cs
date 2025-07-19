// ====================================================================================
// File: WorkItem.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\WorkItems\Entities\WorkItem.cs
// Description: WorkItem entity.
// ====================================================================================

using System;
using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.WorkItems.Entities;

/// <summary>
/// Work Item entity class.
/// </summary>
public class WorkItem : BaseEntity
{
    /// <summary>
    /// ID of the work item.
    /// </summary>
    public WorkItemId Id { get; set; }

    /// <summary>
    /// Title of the work item.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of the work item.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// ID of the engagement that this work item belongs to.
    /// </summary>
    public EngagementId EngagementId { get; set; }

    /// <summary>
    /// Date when the work item is to start.
    /// </summary>
    public DateTimeOffset? PlannedStartDate { get; set; }

    /// <summary>
    /// Date when the work item was completed.
    /// </summary>
    public DateTimeOffset? CompletedDate { get; set; }

    /// <summary>
    /// Status of the work item.
    /// </summary>
    public WorkItemStatus Status { get; set; }

    /// <summary>
    /// Priority of the work item.
    /// </summary>
    public Priority Priority { get; set; } = Priority.Medium;

    /// <summary>
    /// ID of the employee assigned to this work item.
    /// </summary>
    public EmployeeId? AssignedToId { get; set; }

    public DateTimeOffset? PlannedEndDate { get; set; }

    public string? MigrationJobId { get; set; }

    public string? MigrationJobResourceId { get; set; }

    public string? MigrationResourceId { get; set; }

    /// <summary>
    /// Engagement associated with the work item.
    /// </summary>
    public Engagement? Engagement { get; set; }

    /// <summary>
    /// Employee assigned to the work item.
    /// </summary>
    public Employee? AssignedTo { get; set; }

    public override TModel ToModel<TModel>()
    {
        if (typeof(TModel) == typeof(Models.WorkItemModel))
        {
            var model = new Models.WorkItemModel
            {
                Id = Id,
                Title = Title,
                Description = Description ?? string.Empty,
                EngagementId = EngagementId,
                EngagementTitle = Engagement?.Title ?? string.Empty,
                ClientId = Engagement?.ClientId,
                ClientName = Engagement?.Client?.Name ?? string.Empty,
                TenantId = TenantId,
                StartDate = PlannedStartDate,
                CompletedDate = CompletedDate,
                Status = Status,
                Priority = Priority,
                AssignedToId = AssignedToId,
                AssignedToName = GetEmployeeName(AssignedTo),
            };
            return (TModel)(object)model;
        }
        throw new InvalidOperationException($"Cannot map {nameof(WorkItem)} to {typeof(TModel).Name}");
    }

    public override ResourceIdeaResponse<TModel> ToResourceIdeaResponse<TEntity, TModel>()
    {
        // Only WorkItemModel is supported for now
        if (typeof(TModel) == typeof(Models.WorkItemModel))
        {
            return ResourceIdeaResponse<TModel>.Success(ToModel<TModel>());
        }
        throw new InvalidOperationException($"Cannot map {typeof(TEntity).Name} to {typeof(TModel).Name}");
    }

    /// <summary>
    /// Helper method to get the full name of an employee.
    /// </summary>
    /// <param name="employee">The employee entity.</param>
    /// <returns>The full name of the employee or empty string if null.</returns>
    private static string GetEmployeeName(Employee? employee)
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
}

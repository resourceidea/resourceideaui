// ----------------------------------------------------------------------------------
// File: WorkItemModel.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\WorkItems\Models\WorkItemModel.cs
// Description: Work Item model.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.WorkItems.Models;

/// <summary>
/// Represents a work item model.
/// </summary>
public record WorkItemModel
{
    /// <summary>
    /// Gets or sets the work item ID.
    /// </summary>
    public WorkItemId Id { get; init; }

    /// <summary>
    /// Gets or sets the title of the work item.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the work item.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the engagement ID that this work item belongs to.
    /// </summary>
    public EngagementId EngagementId { get; init; }

    /// <summary>
    /// Gets or sets the title of the engagement that this work item belongs to.
    /// </summary>
    public string EngagementTitle { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the tenant ID.
    /// </summary>
    public TenantId TenantId { get; init; }

    /// <summary>
    /// Gets or sets the start date of the work item.
    /// </summary>
    public DateTimeOffset? StartDate { get; init; }

    /// <summary>
    /// Gets or sets the completion date of the work item.
    /// </summary>
    public DateTimeOffset? CompletedDate { get; init; }

    /// <summary>
    /// Gets or sets the status of the work item.
    /// </summary>
    public WorkItemStatus Status { get; init; }

    /// <summary>
    /// Gets or sets the priority of the work item (1-5, where 1 is highest priority).
    /// </summary>
    public int Priority { get; init; } = 3;

    /// <summary>
    /// Gets or sets the employee ID assigned to this work item.
    /// </summary>
    public EmployeeId? AssignedToId { get; init; }

    /// <summary>
    /// Gets or sets the name of the employee assigned to this work item.
    /// </summary>
    public string AssignedToName { get; init; } = string.Empty;
}
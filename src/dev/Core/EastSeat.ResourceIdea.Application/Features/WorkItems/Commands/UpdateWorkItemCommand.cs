using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.WorkItems.Models;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Commands;

/// <summary>
/// Represents a command to update a work item.
/// </summary>
public sealed class UpdateWorkItemCommand : IRequest<ResourceIdeaResponse<WorkItemModel>>
{
    /// <summary>
    /// Gets or sets the work item ID.
    /// </summary>
    public WorkItemId WorkItemId { get; init; }

    /// <summary>
    /// Gets or sets the tenant ID.
    /// </summary>
    public TenantId TenantId { get; set; }

    /// <summary>
    /// Gets or sets the engagement ID.
    /// </summary>
    public EngagementId EngagementId { get; set; }

    /// <summary>
    /// Gets or sets the title of the work item.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the work item.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the start date of the work item.
    /// </summary>
    public DateTimeOffset? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the completion date of the work item.
    /// </summary>
    public DateTimeOffset? CompletedDate { get; set; }

    /// <summary>
    /// Gets or sets the status of the work item.
    /// </summary>
    public WorkItemStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the priority of the work item (1-5, where 1 is highest priority).
    /// </summary>
    public int Priority { get; set; } = 3;

    /// <summary>
    /// Gets or sets the employee ID assigned to this work item.
    /// </summary>
    public EmployeeId? AssignedToId { get; set; }
}
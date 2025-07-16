using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Extensions;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.WorkItems.Models;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;

using System.Linq;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Commands;

/// <summary>
/// Represents a command to update a work item.
/// </summary>
public sealed class UpdateWorkItemCommand : BaseRequest<WorkItemModel>
{
    /// <summary>
    /// Gets or sets the work item ID.
    /// </summary>
    public WorkItemId WorkItemId { get; init; }

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
    /// Gets or sets the priority of the work item.
    /// </summary>
    public Priority Priority { get; set; } = Priority.Medium;

    /// <summary>
    /// Gets or sets the employee ID assigned to this work item.
    /// </summary>
    public EmployeeId? AssignedToId { get; set; }

    /// <summary>
    /// Maps the command to <see cref="WorkItem"/> entity.
    /// </summary>
    /// <returns><see cref="WorkItem"/></returns>
    public WorkItem ToEntity()
    {
        return new WorkItem
        {
            Id = WorkItemId,
            Title = Title,
            Description = Description,
            EngagementId = EngagementId,
            TenantId = TenantId,
            PlannedStartDate = StartDate,
            CompletedDate = CompletedDate,
            Status = Status,
            Priority = Priority,
            AssignedToId = AssignedToId
        };
    }

    /// <summary>
    /// Validates the command.
    /// </summary>
    /// <returns><see cref="ValidationResponse"/></returns>
    public override ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            Title.ValidateRequired(nameof(Title)),
            ValidateEngagementId(),
            TenantId.ValidateRequired(),
            ValidateWorkItemId()
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }

    /// <summary>
    /// Validates the engagement ID.
    /// </summary>
    /// <returns>Validation error message or empty string if valid.</returns>
    private string ValidateEngagementId()
    {
        if (EngagementId.Value == Guid.Empty)
        {
            return "Engagement ID is required.";
        }
        return string.Empty;
    }

    /// <summary>
    /// Validates the work item ID.
    /// </summary>
    /// <returns>Validation error message or empty string if valid.</returns>
    private string ValidateWorkItemId()
    {
        if (WorkItemId.Value == Guid.Empty)
        {
            return "Work Item ID is required.";
        }
        return string.Empty;
    }
}
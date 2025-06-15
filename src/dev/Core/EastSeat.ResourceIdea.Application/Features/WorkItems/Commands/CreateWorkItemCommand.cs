// ----------------------------------------------------------------------------------
// File: CreateWorkItemCommand.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\WorkItems\Commands\CreateWorkItemCommand.cs
// Description: Command to create a work item.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Extensions;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.WorkItems.Models;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Commands;

/// <summary>
/// Represents a command to create a new work item.
/// </summary>
public sealed class CreateWorkItemCommand : BaseRequest<WorkItemModel>
{
    /// <summary>
    /// Gets or sets the title of the work item.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the work item.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the engagement ID that this work item belongs to.
    /// </summary>
    public EngagementId EngagementId { get; set; }

    /// <summary>
    /// Gets or sets the start date of the work item.
    /// </summary>
    public DateTimeOffset? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the completion date of the work item.
    /// </summary>
    public DateTimeOffset? CompletedDate { get; set; }

    /// <summary>
    /// Gets or sets the priority of the work item (1-5, where 1 is highest priority).
    /// </summary>
    public int Priority { get; set; } = 3;

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
            Id = WorkItemId.NewId(),
            Title = Title,
            Description = Description,
            EngagementId = EngagementId,
            TenantId = TenantId,
            StartDate = StartDate,
            CompletedDate = CompletedDate,
            Status = WorkItemStatus.NotStarted,
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
            ValidateStartDate(),
            ValidateCompletedDate()
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
    /// Validates the start date.
    /// </summary>
    /// <returns>Validation error message or empty string if valid.</returns>
    private string ValidateStartDate()
    {
        if (StartDate.HasValue && StartDate.Value.Date < DateTimeOffset.UtcNow.Date)
        {
            return "Start date must be today or in the future.";
        }
        return string.Empty;
    }

    /// <summary>
    /// Validates the completed date.
    /// </summary>
    /// <returns>Validation error message or empty string if valid.</returns>
    private string ValidateCompletedDate()
    {
        if (CompletedDate.HasValue && StartDate.HasValue && CompletedDate.Value < StartDate.Value)
        {
            return "Completed date cannot be earlier than the start date.";
        }
        return string.Empty;
    }
}
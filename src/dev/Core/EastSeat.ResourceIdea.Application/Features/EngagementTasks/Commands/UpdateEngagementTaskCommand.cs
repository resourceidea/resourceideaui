using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;

/// <summary>
/// Represents a command to update an engagement task.
/// </summary>
public sealed class UpdateEngagementTaskCommand : IRequest<ResourceIdeaResponse<EngagementTaskModel>>
{
    /// <summary>
    /// Gets or sets the ID of the engagement task.
    /// </summary>
    public required EngagementTaskId EngagementTaskId { get; init; }

    /// <summary>
    /// Gets or sets the title of the engagement task.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the engagement task.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the engagement associated with the task.
    /// </summary>
    public required EngagementId EngagementId { get; init; }

    /// <summary>
    /// Gets or sets the due date of the engagement task.
    /// </summary>
    public DateTimeOffset? DueDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the engagement task is assigned.
    /// </summary>
    public bool IsAssigned { get; set; }

    /// <summary>
    /// Gets or sets the status of the engagement task.
    /// </summary>
    public EngagementTaskStatus EngagementTaskStatus { get; set; }
}
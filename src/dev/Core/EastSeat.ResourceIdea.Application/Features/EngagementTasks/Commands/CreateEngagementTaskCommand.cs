using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;

/// <summary>
/// Represents a command to start an engagement task.
/// </summary>
public sealed class CreateEngagementTaskCommand : IRequest<ResourceIdeaResponse<EngagementTaskModel>>
{
    /// <summary>
    /// Gets or sets the title of the engagement task.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Gets or sets the description of the engagement task.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets or sets the ID of the engagement associated with the task.
    /// </summary>
    public EngagementId EngagementId { get; set; }

    /// <summary>
    /// Date when the task is expected to be completed.
    /// </summary>
    public DateTimeOffset DueDate { get; set; }
}
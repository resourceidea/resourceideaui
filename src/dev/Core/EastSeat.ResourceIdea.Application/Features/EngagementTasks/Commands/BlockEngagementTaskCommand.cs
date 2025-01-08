using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;

/// <summary>
/// Represents a command to block an engagement task.
/// </summary>
public sealed class BlockEngagementTaskCommand : IRequest<ResourceIdeaResponse<EngagementTaskModel>>
{
    /// <summary>
    /// Gets or sets the ID of the engagement task to block.
    /// </summary>
    public EngagementTaskId EngagementTaskId { get; init; }

    /// <summary>
    /// Gets or sets the reason for blocking the engagement task.
    /// </summary>
    public string? Reason { get; init; }
}
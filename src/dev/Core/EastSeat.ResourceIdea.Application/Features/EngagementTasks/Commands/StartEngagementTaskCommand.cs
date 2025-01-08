using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;

/// <summary>
/// Represents a command to start an engagement task.
/// </summary>
public sealed class StartEngagementTaskCommand : IRequest<ResourceIdeaResponse<EngagementTaskModel>>
{
    /// <summary>
    /// Gets or sets the ID of the engagement task to start.
    /// </summary>
    public EngagementTaskId EngagementTaskId { get; init; }
}
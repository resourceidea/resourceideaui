using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;

/// <summary>
/// Represents a command to remove an engagement task.
/// </summary>
public sealed class RemoveEngagementTaskCommand : IRequest
{
    /// <summary>
    /// Gets or sets the ID of the engagement task to remove.
    /// </summary>
    public EngagementTaskId EngagementTaskId { get; init; }
}
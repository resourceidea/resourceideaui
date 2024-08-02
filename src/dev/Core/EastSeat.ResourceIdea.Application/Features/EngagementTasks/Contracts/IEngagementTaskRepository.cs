using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;

/// <summary>
/// Represents a repository for managing engagement tasks.
/// </summary>
public interface IEngagementTaskRepository : IAsyncRepository<EngagementTask>
{
    /// <summary>
    /// Sets the flag to indicate that an engagement task is assigned to a user.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An optional engagement task if the assignment was successful, otherwise None.</returns>
    Task<Optional<EngagementTask>> SetAssignmentStatusFlagAsync(EngagementTaskId engagementTaskId, CancellationToken cancellationToken);

    /// <summary>
    /// Blocks an engagement task.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task.</param>
    /// <param name="reason">The reason for blocking the task.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An optional engagement task if the blocking was successful, otherwise None.</returns>
    Task<Optional<EngagementTask>> BlockAsync(EngagementTaskId engagementTaskId, string? reason, CancellationToken cancellationToken);

    /// <summary>
    /// Closes an engagement task.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An optional engagement task if the closing was successful, otherwise None.</returns>
    Task<Optional<EngagementTask>> CloseAsync(EngagementTaskId engagementTaskId, CancellationToken cancellationToken);

    /// <summary>
    /// Completes an engagement task.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An optional engagement task if the completion was successful, otherwise None.</returns>
    Task<Optional<EngagementTask>> CompleteAsync(EngagementTaskId engagementTaskId, CancellationToken cancellationToken);

    /// <summary>
    /// Reopens a closed engagement task.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An optional engagement task if the reopening was successful, otherwise None.</returns>
    Task<Optional<EngagementTask>> ReopenAsync(EngagementTaskId engagementTaskId, CancellationToken cancellationToken);

    /// <summary>
    /// Starts an engagement task.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An optional engagement task if the starting was successful, otherwise None.</returns>
    Task<Optional<EngagementTask>> StartAsync(EngagementTaskId engagementTaskId, CancellationToken cancellationToken);
}
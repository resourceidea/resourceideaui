using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;

/// <summary>
/// Represents the service for managing engagement tasks.
/// </summary>
public interface IEngagementTasksService : IDataStoreService<EngagementTask>
{
    /// <summary>
    /// Sets the assignment status flag of an engagement task asynchronously.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{EngagementTask}"/>.</returns>
    Task<ResourceIdeaResponse<EngagementTask>> SetAssignmentStatusFlagAsync(
        EngagementTaskId engagementTaskId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Blocks an engagement task asynchronously.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task.</param>
    /// <param name="reason">The reason for blocking the engagement task.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{EngagementTask}"/>.</returns>
    Task<ResourceIdeaResponse<EngagementTask>> BlockAsync(
        EngagementTaskId engagementTaskId,
        string? reason,
        CancellationToken cancellationToken);

    /// <summary>
    /// Closes an engagement task asynchronously.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{EngagementTask}"/>.</returns>
    Task<ResourceIdeaResponse<EngagementTask>> CloseAsync(
        EngagementTaskId engagementTaskId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Reopens a closed engagement task asynchronously.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{EngagementTask}"/>.</returns>
    Task<ResourceIdeaResponse<EngagementTask>> ReopenAsync(
        EngagementTaskId engagementTaskId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Completes an engagement task asynchronously.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task.</param>
    /// <param name="completionDate">The completion date of the engagement task.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{EngagementTask}"/>.</returns>
    Task<ResourceIdeaResponse<EngagementTask>> CompleteAsync(
        EngagementTaskId engagementTaskId,
        DateTimeOffset completionDate,
        CancellationToken cancellationToken);

    /// <summary>
    /// Starts an engagement asynchronously.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{Engagement}"/>.</returns>
    Task<ResourceIdeaResponse<Engagement>> StartAsync(
        EngagementTaskId engagementTaskId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Assigns the engagement task to a list of application users asynchronously.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task.</param>
    /// <param name="applicationUserIds">The IDs of the application users to assign the engagement task to.</param>
    /// <param name="startDate">The start date of the assignment.</param>
    /// <param name="endDate">The end date of the assignment.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{IReadOnlyList{EngagementTaskAssignment}}"/>.</returns>
    Task<ResourceIdeaResponse<IReadOnlyList<EngagementTaskAssignment>>> AssignAsync(
        EngagementTaskId engagementTaskId,
        IReadOnlyList<ApplicationUserId> applicationUserIds,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        CancellationToken cancellationToken);

    /// <summary>
    /// Unassigns the engagement task from a list of application users asynchronously.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task.</param>
    /// <param name="applicationUserIds">The IDs of the application users to unassign the engagement task from.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{IReadOnlyList{EngagementTaskAssignment}}"/>.</returns>
    Task<ResourceIdeaResponse<IReadOnlyList<EngagementTaskAssignment>>> UnassignAsync(
        EngagementTaskId engagementTaskId,
        IReadOnlyList<ApplicationUserId> applicationUserIds,
        CancellationToken cancellationToken);

    /// <summary>
    /// Remove and engagement task asynchronously.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task to remove.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{EngagementTask}"/></returns>
    Task<ResourceIdeaResponse<EngagementTask>> RemoveAsync(
        EngagementTaskId engagementTaskId,
        CancellationToken cancellationToken);
}

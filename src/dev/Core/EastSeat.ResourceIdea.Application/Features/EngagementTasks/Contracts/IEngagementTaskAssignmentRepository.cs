using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;

/// <summary>
/// Represents a repository for managing engagement task assignments.
/// </summary>
public interface IEngagementTaskAssignmentRepository
{
    /// <summary>
    /// Assigns an engagement task to a user.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task to assign.</param>
    /// <param name="applicationUserId">The ID of the user to assign the task to.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An optional engagement task assignment.</returns>
    Task<Optional<EngagementTaskAssignment>> AssignAsync(
        EngagementTaskId engagementTaskId,
        ApplicationUserId applicationUserId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Unassigns an engagement task from a user.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task to unassign.</param>
    /// <param name="applicationUserId">The ID of the user to unassign the task from.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An optional engagement task assignment.</returns>
    Task<Optional<EngagementTaskAssignment>> UnassignAsync(
        EngagementTaskId engagementTaskId,
        ApplicationUserId applicationUserId,
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Unassigns an engagement task from multiple users.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task to unassign.</param>
    /// <param name="applicationUsers">The IDs of the users to unassign the task from.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UnassignAsync(
        EngagementTaskId engagementTaskId,
        IReadOnlyCollection<ApplicationUserId> applicationUsers,
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Assigns an engagement task to multiple users.
    /// </summary>
    /// <param name="engagementTaskId">The ID of the engagement task to assign.</param>
    /// <param name="applicationUsers">The IDs of the users to assign the task to.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AssignAsync(
        EngagementTaskId engagementTaskId,
        IReadOnlyCollection<ApplicationUserId> applicationUsers,
        CancellationToken cancellationToken);
}
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;

/// <summary>
/// Represents the service for managing engagements.
/// </summary>
public interface IEngagementsService : IDataStoreService<Engagement>
{
    /// <summary>
    /// Cancels an engagement.
    /// </summary>
    /// <param name="engagementId">The ID of the engagement to cancel.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{Engagement}"/>.</returns>
    Task<ResourceIdeaResponse<Engagement>> CancelAsync(
        EngagementId engagementId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Completes an engagement.
    /// </summary>
    /// <param name="engagementId">The ID of the engagement to complete.</param>
    /// <param name="completionDate">The completion date of the engagement.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{Engagement}"/>.</returns>
    Task<ResourceIdeaResponse<Engagement>> CompleteAsync(
        EngagementId engagementId,
        DateTimeOffset completionDate,
        CancellationToken cancellationToken);

    /// <summary>
    /// Starts an engagement.
    /// </summary>
    /// <param name="engagementId">The ID of the engagement to start.</param>
    /// <param name="commencementDate">The commencement date of the engagement.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{Engagement}"/>.</returns>
    Task<ResourceIdeaResponse<Engagement>> StartAsync(
        EngagementId engagementId,
        DateTimeOffset commencementDate,
        CancellationToken cancellationToken);
}

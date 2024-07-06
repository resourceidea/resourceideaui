using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;

/// <summary>
/// Represents the interface for the engagement repository.
/// </summary>
public interface IEngagementRepository : IAsyncRepository<Engagement>
{
    /// <summary>
    /// Cancels an engagement asynchronously.
    /// </summary>
    /// <param name="engagement">The engagement to cancel.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The cancelled engagement.</returns>
    Task<Engagement> CancelAsync(Engagement engagement, CancellationToken cancellationToken);

    /// <summary>
    /// Completes an engagement asynchronously.
    /// </summary>
    /// <param name="engagement">The engagement to complete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The completed engagement.</returns>
    Task<Engagement> CompleteAsync(Engagement engagement, CancellationToken cancellationToken);

    /// <summary>
    /// Starts an engagement.
    /// </summary>
    /// <param name="engagement">The engagement to start.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The started engagement.</returns>
    Task<Engagement> StartAsync(Engagement engagement, CancellationToken cancellationToken);
}

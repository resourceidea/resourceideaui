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
    Task<Engagement> CancelAsync(Engagement engagement, CancellationToken cancellationToken);
}

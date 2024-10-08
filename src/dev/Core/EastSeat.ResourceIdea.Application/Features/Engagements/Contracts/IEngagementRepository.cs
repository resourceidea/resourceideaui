﻿using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;

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
    Task<Optional<Engagement>> CancelAsync(Engagement engagement, CancellationToken cancellationToken);

    /// <summary>
    /// Completes an engagement asynchronously.
    /// </summary>
    /// <param name="engagementId">The ID of the engagement to complete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The completed engagement.</returns>
    Task<Optional<Engagement>> CompleteAsync(EngagementId engagementId, CancellationToken cancellationToken);

    /// <summary>
    /// Starts an engagement.
    /// </summary>
    /// <param name="engagement">The engagement to start.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The started engagement.</returns>
    Task<Optional<Engagement>> StartAsync(Engagement engagement, CancellationToken cancellationToken);
}

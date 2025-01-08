using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Commands;

/// <summary>
/// Represents a command to cancel an engagement.
/// </summary>
public sealed class CancelEngagementCommand : IRequest<ResourceIdeaResponse<EngagementModel>>
{
    /// <summary>
    /// Gets or sets the engagement ID.
    /// </summary>
    public EngagementId EngagementId { get; init; }
}

using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Commands;

/// <summary>
/// Represents a command to create a new engagement.
/// </summary>
public sealed class CreateEngagementCommand : IRequest<EngagementModel>
{
    /// <summary>
    /// Gets or sets the engagement ID.
    /// </summary>
    public EngagementId EngagementId { get; set; }

    /// <summary>
    /// Gets or sets the client ID.
    /// </summary>
    public ClientId ClientId { get; set; }
}
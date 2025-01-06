using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Commands;

/// <summary>
/// Represents a command to update an engagement.
/// </summary>
public sealed class UpdateEngagementCommand : IRequest<ResourceIdeaResponse<EngagementModel>>
{
    /// <summary>
    /// Gets or sets the engagement ID.
    /// </summary>
    public EngagementId EngagementId { get; init; }

    /// <summary>
    /// Gets or sets the client ID.
    /// </summary>
    public ClientId ClientId { get; set; }

    /// <summary>
    /// Gets or sets the engagement status.
    /// </summary>
    public EngagementStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the commencement date of the engagement.
    /// </summary>
    public DateTimeOffset CommencementDate { get; set; }

    /// <summary>
    /// Gets or sets the completion date of the engagement.
    /// </summary>
    public DateTimeOffset CompletionDate { get; set; }

    /// <summary>
    /// Gets or sets the description of the engagement.
    /// </summary>
    public string? Description { get; set; }
}

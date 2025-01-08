using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Queries;

/// <summary>
/// Query to retrieve an engagement by its identifier.
/// </summary>
public sealed class GetEngagementByIdQuery : IRequest<ResourceIdeaResponse<EngagementModel>>
{
    /// <summary>
    /// The identifier of the engagement to retrieve.
    /// </summary>
    public EngagementId EngagementId { get; set; }
}

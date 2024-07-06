using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Queries;

public sealed class GetEngagementByIdQuery : IRequest<ResourceIdeaResponse<EngagementModel>>
{
    /// <summary>
    /// The identifier of the engagement to retrieve.
    /// </summary>
    public EngagementId EngagementId { get; set; }
}

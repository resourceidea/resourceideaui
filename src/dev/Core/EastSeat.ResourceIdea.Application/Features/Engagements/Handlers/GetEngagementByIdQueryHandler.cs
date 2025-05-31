using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Features.Engagements.Queries;
using EastSeat.ResourceIdea.Application.Features.Engagements.Specifications;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers;

/// <summary>
/// Handles the retrieval of an result by its identifier.
/// </summary>
/// <param name="engagementsService">Engagement repository.</param>
/// <param name="mapper">Object mapper.</param>
public sealed class GetEngagementByIdQueryHandler (IEngagementsService engagementsService)
    : IRequestHandler<GetEngagementByIdQuery, ResourceIdeaResponse<EngagementModel>>
{
    private readonly IEngagementsService _engagementsService = engagementsService;

    public async Task<ResourceIdeaResponse<EngagementModel>> Handle(
        GetEngagementByIdQuery request,
        CancellationToken cancellationToken)
    {
        var getEngagementByIdSpecification = new GetEngagementByIdSpecification(request.EngagementId);
        var result = await _engagementsService.GetByIdAsync(getEngagementByIdSpecification, cancellationToken);
        if (result.IsFailure)
        {
            return ResourceIdeaResponse<EngagementModel>.Failure(result.Error);
        }

        if (result.Content != null is false)
        {
            return ResourceIdeaResponse<EngagementModel>.NotFound();
        }

        return result.Content.ToResourceIdeaResponse();
    }
}
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Features.Engagements.Queries;
using EastSeat.ResourceIdea.Application.Features.Engagements.Specifications;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers;

public class GetEngagementsByStatusQueryHandler(IEngagementsService engagementsService)
    : IRequestHandler<GetEngagementsByStatusQuery, ResourceIdeaResponse<PagedListResponse<EngagementModel>>>
{
    private readonly IEngagementsService _engagementsService = engagementsService;

    public async Task<ResourceIdeaResponse<PagedListResponse<EngagementModel>>> Handle(GetEngagementsByStatusQuery request, CancellationToken cancellationToken)
    {
        var getEngagementByIdSpecification = new GetEngagementsByStatusSpecification(request.EngagementStatus);
        var result = await _engagementsService.GetPagedListAsync(
            request.PageNumber,
            request.PageSize,
            getEngagementByIdSpecification,
            cancellationToken);
        if (result.IsFailure)
        {
            return ResourceIdeaResponse<PagedListResponse<EngagementModel>>.Failure(result.Error);
        }

        if (result.Content != null is false)
        {
            return ResourceIdeaResponse<PagedListResponse<EngagementModel>>.NotFound();
        }

        return result.Content.ToResourceIdeaResponse();
    }
}
using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Features.Engagements.Queries;
using EastSeat.ResourceIdea.Application.Features.Engagements.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers;

public class GetEngagementsByStatusQueryHandler(
    IEngagementsService engagementsService,
    IMapper mapper) : IRequestHandler<GetEngagementsByStatusQuery, ResourceIdeaResponse<PagedListResponse<EngagementModel>>>
{
    private readonly IEngagementsService _engagementsService = engagementsService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<PagedListResponse<EngagementModel>>> Handle(GetEngagementsByStatusQuery request, CancellationToken cancellationToken)
    {
        var getEngagementByIdSpecification = new GetEngagementsByStatusSpecification(request.EngagementStatus);
        var engagements = await _engagementsService.GetPagedListAsync(
            request.PageNumber,
            request.PageSize,
            getEngagementByIdSpecification,
            cancellationToken);

        return _mapper.Map<ResourceIdeaResponse<PagedListResponse<EngagementModel>>>(engagements);
    }
}
using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Features.Engagements.Queries;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers;

public class GetAllEngagementsQueryHandler(
    IEngagementRepository engagementRepository,
    IMapper mapper
) : IRequestHandler<GetAllEngagementsQuery, ResourceIdeaResponse<PagedListResponse<EngagementModel>>>
{
    private readonly IEngagementRepository _engagementRepository = engagementRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<PagedListResponse<EngagementModel>>> Handle(GetAllEngagementsQuery request, CancellationToken cancellationToken)
    {
        var engagements = await _engagementRepository.GetPagedListAsync(
            request.PageNumber,
            request.PageSize,
            Optional<BaseSpecification<Engagement>>.None,
            cancellationToken);

        return ResourceIdeaResponse<PagedListResponse<EngagementModel>>
                    .Success(Optional<PagedListResponse<EngagementModel>>.Some(_mapper.Map<PagedListResponse<EngagementModel>>(engagements)));
    }
}
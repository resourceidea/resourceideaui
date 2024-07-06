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
    IEngagementRepository engagementRepository,
    IMapper mapper) : IRequestHandler<GetEngagementsByStatusQuery, ResourceIdeaResponse<PagedListResponse<EngagementModel>>>
{
    private readonly IEngagementRepository _engagementRepository = engagementRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<PagedListResponse<EngagementModel>>> Handle(GetEngagementsByStatusQuery request, CancellationToken cancellationToken)
    {
        var getEngagementByIdSpecification = new GetEngagementsByStatusSpecification(request.EngagementStatus);
        var engagements = await _engagementRepository.GetPagedListAsync(
            request.PageNumber,
            request.PageSize,
            getEngagementByIdSpecification,
            cancellationToken);

        return new ResourceIdeaResponse<PagedListResponse<EngagementModel>>
        {
            Success = true,
            Content = Optional<PagedListResponse<EngagementModel>>.Some(_mapper.Map<PagedListResponse<EngagementModel>>(engagements))
        };
    }
}
using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Features.Engagements.Queries;
using EastSeat.ResourceIdea.Application.Features.Engagements.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers;

/// <summary>
/// Handles the retrieval of an engagement by its identifier.
/// </summary>
/// <param name="engagementRepository">Engagement repository.</param>
/// <param name="mapper">Object mapper.</param>
public sealed class GetEngagementByIdQueryHandler (
    IEngagementRepository engagementRepository,
    IMapper mapper) : IRequestHandler<GetEngagementByIdQuery, ResourceIdeaResponse<EngagementModel>>
{
    private readonly IEngagementRepository _engagementRepository = engagementRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<EngagementModel>> Handle(
        GetEngagementByIdQuery request,
        CancellationToken cancellationToken)
    {
        var getEngagementByIdSpecification = new GetEngagementByIdSpecification(request.EngagementId);
        var engagement = await _engagementRepository.GetByIdAsync(getEngagementByIdSpecification, cancellationToken);

        return ResourceIdeaResponse<EngagementModel>
                    .Success(Optional<EngagementModel>.Some(_mapper.Map<EngagementModel>(engagement)));
    }
}
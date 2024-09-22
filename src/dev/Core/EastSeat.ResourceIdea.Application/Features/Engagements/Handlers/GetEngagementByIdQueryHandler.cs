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
/// <param name="engagementsService">Engagement repository.</param>
/// <param name="mapper">Object mapper.</param>
public sealed class GetEngagementByIdQueryHandler (
    IEngagementsService engagementsService,
    IMapper mapper) : IRequestHandler<GetEngagementByIdQuery, ResourceIdeaResponse<EngagementModel>>
{
    private readonly IEngagementsService _engagementsService = engagementsService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<EngagementModel>> Handle(
        GetEngagementByIdQuery request,
        CancellationToken cancellationToken)
    {
        var getEngagementByIdSpecification = new GetEngagementByIdSpecification(request.EngagementId);
        var engagement = await _engagementsService.GetByIdAsync(getEngagementByIdSpecification, cancellationToken);

        return _mapper.Map<ResourceIdeaResponse<EngagementModel>>(engagement);
    }
}
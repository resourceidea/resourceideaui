using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers;

public sealed class CreateEngagementCommandHandler (
    IEngagementsService engagementsService,
    IMapper mapper) : IRequestHandler<CreateEngagementCommand, ResourceIdeaResponse<EngagementModel>>
{
    private readonly IEngagementsService _engagementsService = engagementsService;
    private readonly IMapper _mapper = mapper;
    
    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<EngagementModel>> Handle(
        CreateEngagementCommand request,
        CancellationToken cancellationToken)
    {
        Engagement engagement = new()
        {
            ClientId = request.ClientId,
            Description = request.Description
        };

        var result = await _engagementsService.AddAsync(engagement, cancellationToken);
        if (result.IsFailure)
        {
            return ResourceIdeaResponse<EngagementModel>.Failure(result.Error);
        }

        return _mapper.Map<ResourceIdeaResponse<EngagementModel>>(result);
    }
}

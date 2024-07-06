using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers;

public sealed class CreateEngagementCommandHandler (
    IEngagementRepository engagementRepository,
    IMapper mapper) : IRequestHandler<CreateEngagementCommand, ResourceIdeaResponse<EngagementModel>>
{
    private readonly IEngagementRepository _engagementRepository = engagementRepository;
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

        Engagement createdEngagement = await _engagementRepository.AddAsync(engagement, cancellationToken);

        return new ResourceIdeaResponse<EngagementModel>
        {
            Success = true,
            Message = $"Engagement created successfully.",
            Content = Optional<EngagementModel>.Some(_mapper.Map<EngagementModel>(createdEngagement))
        };
    }
}

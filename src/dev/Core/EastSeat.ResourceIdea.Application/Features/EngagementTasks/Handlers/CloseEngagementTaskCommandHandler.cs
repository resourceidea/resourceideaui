using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Handlers;

public sealed class CloseEngagementTaskCommandHandler (IEngagementTaskRepository repository, IMapper mapper)
    : IRequestHandler<CloseEngagementTaskCommand, ResourceIdeaResponse<EngagementTaskModel>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IEngagementTaskRepository _repository = repository;

    public async Task<ResourceIdeaResponse<EngagementTaskModel>> Handle(CloseEngagementTaskCommand request, CancellationToken cancellationToken)
    {
        var closedEngagementTask = await _repository.CloseAsync(request.EngagementTaskId, cancellationToken);

        return new ResourceIdeaResponse<EngagementTaskModel>
        {
            Success = true,
            Message = $"Engagement task closed successfully.",
            Content = Optional<EngagementTaskModel>.Some(_mapper.Map<EngagementTaskModel>(closedEngagementTask))
        };
    }
}
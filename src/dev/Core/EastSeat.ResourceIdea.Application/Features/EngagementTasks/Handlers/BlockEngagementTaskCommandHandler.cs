using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Handlers;

public sealed class BlockEngagementTaskCommandHandler(IEngagementTaskRepository repository, IMapper mapper)
    : IRequestHandler<BlockEngagementTaskCommand, ResourceIdeaResponse<EngagementTaskModel>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IEngagementTaskRepository _repository = repository;

    public async Task<ResourceIdeaResponse<EngagementTaskModel>> Handle(
        BlockEngagementTaskCommand request,
        CancellationToken cancellationToken)
    {
        var blockedEngagementTask = await _repository.BlockAsync(request.EngagementTaskId, request.Reason, cancellationToken);

        return new ResourceIdeaResponse<EngagementTaskModel>
        {
            Success = true,
            Message = $"Engagement task blocked successfully.",
            Content = Optional<EngagementTaskModel>.Some(_mapper.Map<EngagementTaskModel>(blockedEngagementTask))
        };
    }
}

using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Handlers;

public sealed class StartEngagementTaskCommandHandler(
    IEngagementTasksService engagementTasksService,
    IMapper mapper) : IRequestHandler<StartEngagementTaskCommand, ResourceIdeaResponse<EngagementTaskModel>>
{
    private readonly IEngagementTasksService _engagementTasksService = engagementTasksService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<EngagementTaskModel>> Handle(StartEngagementTaskCommand request, CancellationToken cancellationToken)
    {
        var result = await _engagementTasksService.StartAsync(request.EngagementTaskId, cancellationToken);
        if (result.IsFailure)
        {
            return ResourceIdeaResponse<EngagementTaskModel>.Failure(result.Error);
        }

        return _mapper.Map<ResourceIdeaResponse<EngagementTaskModel>>(result.Content);
    }
}
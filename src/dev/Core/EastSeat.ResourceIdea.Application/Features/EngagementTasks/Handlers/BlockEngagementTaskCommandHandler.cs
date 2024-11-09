using AutoMapper;

using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Handlers;

public sealed class BlockEngagementTaskCommandHandler(
    IEngagementTasksService engagementTasksService,
    IMapper mapper) : IRequestHandler<BlockEngagementTaskCommand, ResourceIdeaResponse<EngagementTaskModel>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IEngagementTasksService _engagementTasksService = engagementTasksService;

    public async Task<ResourceIdeaResponse<EngagementTaskModel>> Handle(
        BlockEngagementTaskCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _engagementTasksService.BlockAsync(
            request.EngagementTaskId,
            request.Reason,
            cancellationToken);

        if (result.IsFailure)
        {
            return ResourceIdeaResponse<EngagementTaskModel>.Failure(ErrorCode.DataStoreCommandFailure);
        }

        return _mapper.Map<ResourceIdeaResponse<EngagementTaskModel>>(result);
    }
}

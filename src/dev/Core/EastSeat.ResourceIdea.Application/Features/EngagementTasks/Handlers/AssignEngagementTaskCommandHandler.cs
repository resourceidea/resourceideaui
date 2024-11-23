using AutoMapper;
using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Handlers;

public sealed class AssignEngagementTaskCommandHandler(
    IEngagementTasksService engagementTasksService,
    IMapper mapper) : IRequestHandler<AssignEngagementTaskCommand, ResourceIdeaResponse<EngagementTaskModel>>
{
    private readonly IEngagementTasksService _engagementTasksService = engagementTasksService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<EngagementTaskModel>> Handle(
        AssignEngagementTaskCommand request,
        CancellationToken cancellationToken)
    {
        // TODO: Add validation of the command to assign engagement task.
        var result = await _engagementTasksService.AssignAsync(
            request.EngagementTaskId,
            request.ApplicationUserIds,
            request.StartDate,
            request.EndDate,
            cancellationToken);

        if (result.IsFailure)
        {
            return ResourceIdeaResponse<EngagementTaskModel>.Failure(ErrorCode.DataStoreCommandFailure);
        }

        return _mapper.Map<ResourceIdeaResponse<EngagementTaskModel>>(result);
    }
}
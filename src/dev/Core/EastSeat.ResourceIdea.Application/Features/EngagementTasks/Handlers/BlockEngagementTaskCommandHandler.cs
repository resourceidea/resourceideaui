using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Handlers;

/// <summary>
/// Handles the command to block an engagement task.
/// </summary>
/// <param name="engagementTasksService">The service for managing engagement tasks.</param>
public sealed class BlockEngagementTaskCommandHandler(IEngagementTasksService engagementTasksService)
    : BaseHandler, IRequestHandler<BlockEngagementTaskCommand, ResourceIdeaResponse<EngagementTaskModel>>
{
    private readonly IEngagementTasksService _engagementTasksService = engagementTasksService;

    /// <summary>
    /// Handles the command to block an engagement task.
    /// </summary>
    /// <param name="request">The command request containing the engagement task ID and reason for blocking.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A response containing the blocked engagement task model or an error code.</returns>
    public async Task<ResourceIdeaResponse<EngagementTaskModel>> Handle(
        BlockEngagementTaskCommand request,
        CancellationToken cancellationToken)
    {
        ResourceIdeaResponse<EngagementTask> result = await _engagementTasksService.BlockAsync(
            request.EngagementTaskId,
            request.Reason,
            cancellationToken);

        var handlerResponse = GetHandlerResponse<EngagementTask, EngagementTaskModel>(result, ErrorCode.EmptyEntityOnBlockEngagementTask);

        return handlerResponse;
    }
}

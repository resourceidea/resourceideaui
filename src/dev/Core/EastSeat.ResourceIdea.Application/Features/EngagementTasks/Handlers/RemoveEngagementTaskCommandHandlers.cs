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
/// Handles the command to remove an engagement task.
/// </summary>
/// <param name="engagementTasksService">The service for managing engagement tasks.</param>
public sealed class RemoveEngagementTaskCommandHandlers(IEngagementTasksService engagementTasksService)
    : BaseHandler, IRequestHandler<RemoveEngagementTaskCommand, ResourceIdeaResponse<EngagementTaskModel>>
{
    private readonly IEngagementTasksService _engagementTasksService = engagementTasksService;

    /// <summary>
    /// Handles the request to remove an engagement task.
    /// </summary>
    /// <param name="request">The command to remove an engagement task.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response with the engagement task model.</returns>
    public async Task<ResourceIdeaResponse<EngagementTaskModel>> Handle(RemoveEngagementTaskCommand request, CancellationToken cancellationToken)
    {
        ResourceIdeaResponse<EngagementTask> response = await _engagementTasksService.RemoveAsync(request.EngagementTaskId, cancellationToken);
        var handlerResponse = GetHandlerResponse<EngagementTask, EngagementTaskModel>(response, ErrorCode.EmptyEntityOnRemoveEngagementTask);

        return handlerResponse;
    }
}

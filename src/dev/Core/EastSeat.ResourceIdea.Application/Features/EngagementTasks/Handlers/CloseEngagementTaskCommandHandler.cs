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
/// Handles the command to close an engagement task.
/// </summary>
/// <param name="engagementTasksService">The service for managing engagement tasks.</param>
public sealed class CloseEngagementTaskCommandHandler(IEngagementTasksService engagementTasksService)
    : BaseHandler, IRequestHandler<CloseEngagementTaskCommand, ResourceIdeaResponse<EngagementTaskModel>>
{
    private readonly IEngagementTasksService _engagementTasksService = engagementTasksService;

    /// <summary>
    /// Handles the request to close an engagement task.
    /// </summary>
    /// <param name="request">The command to close the engagement task.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A response containing the closed engagement task model or an error code.</returns>
    public async Task<ResourceIdeaResponse<EngagementTaskModel>> Handle(CloseEngagementTaskCommand request, CancellationToken cancellationToken)
    {
        ResourceIdeaResponse<EngagementTask> response = await _engagementTasksService.CloseAsync(request.EngagementTaskId, cancellationToken);
        var handlerResponse = GetHandlerResponse<EngagementTask, EngagementTaskModel>(response, ErrorCode.EmptyEntityOnCloseEngagementTask);

        return handlerResponse;
    }
}

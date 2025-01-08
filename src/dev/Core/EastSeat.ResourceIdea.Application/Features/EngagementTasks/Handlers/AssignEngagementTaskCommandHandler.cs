using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Handlers;


/// <summary>
/// Handles the command to assign an engagement task.
/// </summary>
/// <param name="engagementTasksService">The service for managing engagement tasks.</param>
public sealed class AssignEngagementTaskCommandHandler(IEngagementTasksService engagementTasksService)
    : BaseHandler, IRequestHandler<AssignEngagementTaskCommand, ResourceIdeaResponse<IReadOnlyList<EngagementTaskAssignmentModel>>>
{
    private readonly IEngagementTasksService _engagementTasksService = engagementTasksService;

    /// <summary>
    /// Handles the command to assign an engagement task.
    /// </summary>
    /// <param name="request">The command request containing the details of the engagement task assignment.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A response containing the result of the engagement task assignment.</returns>
    public async Task<ResourceIdeaResponse<IReadOnlyList<EngagementTaskAssignmentModel>>> Handle(
        AssignEngagementTaskCommand request,
        CancellationToken cancellationToken)
    {
        // TODO: Add validation of the command to assign engagement task.
        ResourceIdeaResponse<IReadOnlyList<EngagementTaskAssignment>> response = await _engagementTasksService.AssignAsync(
            request.EngagementTaskId,
            request.ApplicationUserIds,
            request.StartDate,
            request.EndDate,
            cancellationToken);

        var handlerResponse = GetHandlerResponse<EngagementTaskAssignment, EngagementTaskAssignmentModel>(response);

        return handlerResponse;
    }
}

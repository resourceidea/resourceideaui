using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Validators;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Handlers;


/// <summary>
/// Handles the command to update an engagement task.
/// </summary>
/// <param name="engagementTasksService">The service for managing engagement tasks.</param>
public sealed class UpdateEngagementTaskCommandHandler(IEngagementTasksService engagementTasksService)
    : BaseHandler, IRequestHandler<UpdateEngagementTaskCommand, ResourceIdeaResponse<EngagementTaskModel>>
{
    private readonly IEngagementTasksService _engagementTasksService = engagementTasksService;

    /// <summary>
    /// Handles the update engagement task command.
    /// </summary>
    /// <param name="request">The update engagement task command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response with the updated engagement task model.</returns>
    public async Task<ResourceIdeaResponse<EngagementTaskModel>> Handle(
        UpdateEngagementTaskCommand request,
        CancellationToken cancellationToken)
    {
        UpdateEngagementTaskCommandValidator _validator = new();
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid is false)
        {
            return ResourceIdeaResponse<EngagementTaskModel>.BadRequest();
        }

        EngagementTask engagementTask = request.ToEntity();
        ResourceIdeaResponse<EngagementTask> response = await _engagementTasksService.UpdateAsync(engagementTask, cancellationToken);
        var handlerResponse = GetHandlerResponse<EngagementTask, EngagementTaskModel>(response, ErrorCode.EmptyEntityOnUpdateEngagementTask);

        return handlerResponse;
    }
}

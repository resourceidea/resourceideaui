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
/// Handles the command to create an engagement task.
/// </summary>
/// <param name="engagementTasksService"></param>
public class CreateEngagementTaskCommandHandler (IEngagementTasksService engagementTasksService)
    : BaseHandler, IRequestHandler<CreateEngagementTaskCommand, ResourceIdeaResponse<EngagementTaskModel>>
{
    private readonly IEngagementTasksService _engagementTasksService = engagementTasksService;

    public async Task<ResourceIdeaResponse<EngagementTaskModel>> Handle(
        CreateEngagementTaskCommand request,
        CancellationToken cancellationToken)
    {
        CreateEngagementTaskCommandValidator _validator = new();
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid is false)
        {
            return ResourceIdeaResponse<EngagementTaskModel>.BadRequest();
        }

        ResourceIdeaResponse<EngagementTask> response = await _engagementTasksService.AddAsync(request.ToEntity(), cancellationToken);

        var handlerResponse = GetHandlerResponse<EngagementTask, EngagementTaskModel>(response, ErrorCode.EmptyEntityOnCreateEngagementTask);

        return handlerResponse;
    }
}
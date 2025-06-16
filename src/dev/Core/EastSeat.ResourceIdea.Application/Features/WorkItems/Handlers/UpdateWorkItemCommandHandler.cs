using EastSeat.ResourceIdea.Application.Features.WorkItems.Commands;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Contracts;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Validators;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.WorkItems.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Handlers;

/// <summary>
/// Handles the update work item command.
/// </summary>
/// <param name="workItemsService">Work items service.</param>
public sealed class UpdateWorkItemCommandHandler(IWorkItemsService workItemsService)
    : IRequestHandler<UpdateWorkItemCommand, ResourceIdeaResponse<WorkItemModel>>
{
    private readonly IWorkItemsService _workItemsService = workItemsService;
    
    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<WorkItemModel>> Handle(
        UpdateWorkItemCommand request,
        CancellationToken cancellationToken)
    {
        UpdateWorkItemCommandValidator updateWorkItemValidator = new();
        var validationResult = updateWorkItemValidator.Validate(request);
        
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            return ResourceIdeaResponse<WorkItemModel>.Failure(ErrorCode.BadRequest);
        }

        WorkItem workItem = request.ToEntity();
        ResourceIdeaResponse<WorkItem> response = await _workItemsService.UpdateAsync(workItem, cancellationToken);
        if (response.IsFailure)
        {
            return ResourceIdeaResponse<WorkItemModel>.Failure(response.Error);
        }

        if (response.Content.HasValue is false)
        {
            return ResourceIdeaResponse<WorkItemModel>.Failure(ErrorCode.NotFound);
        }

        return response.Content.Value.ToResourceIdeaResponse();
    }
}
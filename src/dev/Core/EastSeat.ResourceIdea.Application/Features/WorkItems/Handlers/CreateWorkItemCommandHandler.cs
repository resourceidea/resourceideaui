// ----------------------------------------------------------------------------------
// File: CreateWorkItemCommandHandler.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\WorkItems\Handlers\CreateWorkItemCommandHandler.cs
// Description: Handler for the CreateWorkItemCommand.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.WorkItems.Commands;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Contracts;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.WorkItems.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Handlers;

/// <summary>
/// Handler for the CreateWorkItemCommand.
/// </summary>
public sealed class CreateWorkItemCommandHandler(IWorkItemsService workItemsService)
    : IRequestHandler<CreateWorkItemCommand, ResourceIdeaResponse<WorkItemModel>>
{
    private readonly IWorkItemsService _workItemsService = workItemsService;

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<WorkItemModel>> Handle(
        CreateWorkItemCommand request,
        CancellationToken cancellationToken)
    {
        WorkItem workItem = request.ToEntity();
        var result = await _workItemsService.AddAsync(workItem, cancellationToken);
        if (result.IsFailure)
        {
            return ResourceIdeaResponse<WorkItemModel>.Failure(result.Error);
        }

        if (result.Content.HasValue is false)
        {
            return ResourceIdeaResponse<WorkItemModel>.Failure(ErrorCode.EmptyEntityOnCreateWorkItem);
        }

        return result.Content.Value.ToResourceIdeaResponse();
    }
}
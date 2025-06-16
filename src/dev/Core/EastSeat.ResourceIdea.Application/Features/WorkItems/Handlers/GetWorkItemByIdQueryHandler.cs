using EastSeat.ResourceIdea.Application.Features.WorkItems.Contracts;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Queries;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Specifications;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.WorkItems.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Handlers;

/// <summary>
/// Handles the retrieval of a work item by its identifier.
/// </summary>
/// <param name="workItemsService">Work items service.</param>
public sealed class GetWorkItemByIdQueryHandler(IWorkItemsService workItemsService)
    : IRequestHandler<GetWorkItemByIdQuery, ResourceIdeaResponse<WorkItemModel>>
{
    private readonly IWorkItemsService _workItemsService = workItemsService;

    public async Task<ResourceIdeaResponse<WorkItemModel>> Handle(
        GetWorkItemByIdQuery request,
        CancellationToken cancellationToken)
    {
        var getWorkItemByIdSpecification = new GetWorkItemByIdSpecification(request.WorkItemId, request.TenantId);
        var result = await _workItemsService.GetByIdAsync(getWorkItemByIdSpecification, cancellationToken);
        if (result.IsFailure)
        {
            return ResourceIdeaResponse<WorkItemModel>.Failure(result.Error);
        }

        if (result.Content.HasValue is false)
        {
            return ResourceIdeaResponse<WorkItemModel>.NotFound();
        }

        return result.Content.Value.ToResourceIdeaResponse();
    }
}
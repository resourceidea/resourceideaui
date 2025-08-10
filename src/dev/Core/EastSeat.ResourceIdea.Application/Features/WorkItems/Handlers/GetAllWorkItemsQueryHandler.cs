using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Contracts;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Queries;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Specifications;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.WorkItems.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Handlers;

public class GetAllWorkItemsQueryHandler(IWorkItemsService workItemsService)
    : BaseHandler,
      IRequestHandler<GetAllWorkItemsQuery, ResourceIdeaResponse<PagedListResponse<WorkItemModel>>>
{
    private readonly IWorkItemsService _workItemsService = workItemsService;

    public async Task<ResourceIdeaResponse<PagedListResponse<WorkItemModel>>> Handle(GetAllWorkItemsQuery request, CancellationToken cancellationToken)
    {
        TenantWorkItemsSpecification specification = new(request.TenantId);
        var result = await _workItemsService.GetPagedListAsync(
            request.PageNumber,
            request.PageSize,
            specification,
            cancellationToken);
        if (result.IsFailure)
        {
            return ResourceIdeaResponse<PagedListResponse<WorkItemModel>>.Failure(result.Error);
        }

        if (result.Content.HasValue is false)
        {
            return ResourceIdeaResponse<PagedListResponse<WorkItemModel>>.NotFound();
        }

        return result.Content.Value.ToResourceIdeaResponse();
    }
}
using EastSeat.ResourceIdea.Domain.WorkItems.Models;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Queries;

/// <summary>
/// Query to retrieve a work item by its identifier.
/// </summary>
public sealed class GetWorkItemByIdQuery : IRequest<ResourceIdeaResponse<WorkItemModel>>
{
    /// <summary>
    /// The identifier of the work item to retrieve.
    /// </summary>
    public WorkItemId WorkItemId { get; set; }
}
using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Specifications;

/// <summary>
/// Specification to retrieve a work item by its identifier.
/// </summary>
/// <param name="workItemId">Work item identifier.</param>
public class GetWorkItemByIdSpecification(WorkItemId workItemId) : BaseSpecification<WorkItem>
{
    private readonly WorkItemId _workItemId = workItemId;

    /// <summary>
    /// Criteria to retrieve a work item by its identifier.
    /// </summary>
    public override Expression<Func<WorkItem, bool>> Criteria => workItem => workItem.Id == _workItemId;
}
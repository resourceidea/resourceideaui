using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Specifications;

/// <summary>
/// Specification to retrieve a work item by its identifier and tenant.
/// </summary>
/// <param name="workItemId">Work item identifier.</param>
/// <param name="tenantId">Tenant identifier.</param>
public class GetWorkItemByIdSpecification(WorkItemId workItemId, TenantId tenantId) : BaseSpecification<WorkItem>
{
    private readonly WorkItemId _workItemId = workItemId;
    private readonly TenantId _tenantId = tenantId;

    /// <summary>
    /// Criteria to retrieve a work item by its identifier and tenant.
    /// </summary>
    public override Expression<Func<WorkItem, bool>> Criteria => workItem => workItem.Id == _workItemId && workItem.TenantId == _tenantId;
}
// =============================================================================================================
// File: WorkItemsByEngagementSpecification.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\WorkItems\Specifications\WorkItemsByEngagementSpecification.cs
// Description: Specification for retrieving work items by engagement
// =============================================================================================================

using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Specifications;

/// <summary>
/// Specification to filter work items by a specific engagement identifier and tenant.
/// </summary>
/// <remarks>
/// This specification returns work items associated with the provided <see cref="EngagementId"/> and <see cref="TenantId"/>.
/// </remarks>
/// <param name="engagementId">The identifier of the engagement to filter work items by.</param>
/// <param name="tenantId">The identifier of the tenant to filter work items by.</param>
public sealed class WorkItemsByEngagementSpecification(EngagementId engagementId, TenantId tenantId) : BaseSpecification<WorkItem>
{
    public EngagementId EngagementId => engagementId;
    public TenantId TenantId => tenantId;

    public override Expression<Func<WorkItem, bool>> Criteria =>
        workItem => workItem.EngagementId == engagementId && workItem.TenantId == tenantId;
}
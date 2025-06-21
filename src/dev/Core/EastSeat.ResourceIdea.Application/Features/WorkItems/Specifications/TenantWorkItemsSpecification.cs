// =============================================================================================================
// File: TenantWorkItemsSpecification.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\WorkItems\Specifications\TenantWorkItemsSpecification.cs
// Description: Specification for retrieving tenant work items
// =============================================================================================================

using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Specifications;

/// <summary>
/// Specification to filter work items by a specific tenant identifier.
/// </summary>
/// <remarks>
/// This specification returns work items associated with the provided <see cref="TenantId"/>.
/// </remarks>
/// <param name="tenantId">The identifier of the tenant to filter work items by.</param>
public sealed class TenantWorkItemsSpecification(TenantId tenantId) : BaseSpecification<WorkItem>
{
    public TenantId TenantId => tenantId;

    public override Expression<Func<WorkItem, bool>> Criteria =>
        workItem => workItem.TenantId == tenantId;
}
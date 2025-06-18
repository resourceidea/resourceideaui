// =============================================================================================================
// File: WorkItemsByClientSpecification.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\WorkItems\Specifications\WorkItemsByClientSpecification.cs
// Description: Specification for retrieving work items by client
// =============================================================================================================

using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Specifications;

/// <summary>
/// Specification to filter work items by a specific client identifier and tenant.
/// </summary>
/// <remarks>
/// This specification returns work items associated with engagements that belong to the provided <see cref="ClientId"/> and <see cref="TenantId"/>.
/// </remarks>
/// <param name="clientId">The identifier of the client to filter work items by.</param>
/// <param name="tenantId">The identifier of the tenant to filter work items by.</param> 
public sealed class WorkItemsByClientSpecification(ClientId clientId, TenantId tenantId) : BaseSpecification<WorkItem>
{
    public ClientId ClientId => clientId;
    public TenantId TenantId => tenantId;

    public override Expression<Func<WorkItem, bool>> Criteria =>
        workItem => workItem.Engagement != null && 
                    workItem.Engagement.ClientId == clientId && 
                    workItem.TenantId == tenantId;
}
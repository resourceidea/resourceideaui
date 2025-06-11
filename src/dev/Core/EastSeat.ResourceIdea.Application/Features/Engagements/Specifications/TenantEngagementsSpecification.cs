// =============================================================================================================
// File: TenantEngagementsSpecification.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Engagements\Specifications\TenantEngagementsSpecification.cs
// Description: Specification for retrieving tenant engagements
// =============================================================================================================

using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Specifications;

/// <summary>
/// Specification to filter engagements by a specific tenant identifier.
/// </summary>
/// <remarks>
/// This specification returns engagements associated with the provided <see cref="TenantId"/>.
/// </remarks>
/// <param name="tenantId">The identifier of the tenant to filter engagements by.</param> 
public sealed class TenantEngagementsSpecification(TenantId tenantId) : BaseSpecification<Engagement>
{
    public TenantId TenantId => tenantId;

    public override Expression<Func<Engagement, bool>> Criteria =>
        engagement => engagement.TenantId == tenantId;
}
using System.Linq.Expressions;

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Specifications;

/// <summary>
/// Specification to get a tenant by Id.
/// </summary>
/// <param name="tenantId">Tenant Id.</param>
public sealed class TenantGetByIdSpecification(TenantId tenantId) : BaseSpecification<Tenant>
{
    private readonly TenantId _tenantId = tenantId;

    public override Expression<Func<Tenant, bool>> Criteria => tenant => tenant.TenantId == _tenantId.Value;
}

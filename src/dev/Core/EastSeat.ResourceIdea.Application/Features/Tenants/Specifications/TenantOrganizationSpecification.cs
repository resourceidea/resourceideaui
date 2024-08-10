using System.Linq.Expressions;

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Specifications;

/// <summary>
/// Specification for filtering tenants whose organization name contains the filter value.
/// </summary>
/// <param name="filters">Values used by the specification to filter the values returned when querying for tenants.</param>
public sealed class TenantOrganizationSpecification(Dictionary<string, string> filters)
        : BaseStringSpecification<Tenant>(filters)
{
    protected override Expression<Func<Tenant, bool>> GetExpression() => tenant => tenant.Organization.Contains(_filters[GetFilterKey()]);

    protected override string GetFilterKey() => "organization";
}

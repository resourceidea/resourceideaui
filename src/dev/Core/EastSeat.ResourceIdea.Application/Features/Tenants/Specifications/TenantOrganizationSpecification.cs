using System.Linq.Expressions;

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;

using Optional;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Specifications;

/// <summary>
/// Specification for filtering tenants whose organization name contains the filter value.
/// </summary>
/// <param name="filters">Values used by the specification to filter the values returned when querying for tenants.</param>
public sealed class TenantOrganizationSpecification(Dictionary<string, string>? filters)
    : BaseSpecification<Tenant>
{
    private readonly Dictionary<string, string>? _filters = filters;

    public override Expression<Func<Tenant, bool>> Criteria
    {
        get
        {
            Option<string> filterValidationResult = GetValidatedOrganizationFilter();
            string filter = filterValidationResult.Match(
                some: value => value,
                none: () => string.Empty);

            return string.IsNullOrEmpty(filter)
                ? tenant => false
                : tenant => tenant.Organization.Contains(filter);
        }
    }

    private Option<string> GetValidatedOrganizationFilter()
    {
        if (_filters is null
            || _filters.Count <= 0
            || !_filters.TryGetValue("organization", out var organizationValue)
            || string.IsNullOrEmpty(organizationValue))
        {
            return Option.None<string>();
        }

        return Option.Some<string>(organizationValue);
    }
}

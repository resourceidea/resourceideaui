using System.Linq.Expressions;

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Specifications;

public sealed class TenantOrganizationSpecification(string organization) : BaseSpecification<Tenant>
{
    private readonly string _organization = organization;

    public override Expression<Func<Tenant, bool>> Criteria => tenant => tenant.Organization.Contains(_organization);
}

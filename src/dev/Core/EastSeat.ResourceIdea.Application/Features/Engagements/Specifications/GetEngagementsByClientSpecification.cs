using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Specifications;

/// <summary>
/// Specification to retrieve an engagement by the owning client.
/// </summary>
/// <param name="clientId">Client identifier.</param>
/// <param name="tenantId">Tenant identifier.</param>
/// <param name="searchTerm">Optional search term to filter engagements.</param>
public class GetEngagementsByClientSpecification(ClientId clientId, TenantId tenantId, string? searchTerm = null)
    : BaseSpecification<Engagement>
{
    private readonly ClientId _clientId = clientId;
    private readonly TenantId _tenantId = tenantId;
    private readonly string? _searchTerm = searchTerm;

    /// <summary>
    /// Criteria to retrieve an engagements by its owning client.
    /// </summary>
    public override Expression<Func<Engagement, bool>> Criteria
    {
        get
        {
            // engagement => engagement.ClientId == _clientId
            //            && engagement.TenantId == _tenantId
            //            && engagement.Description != null
            //            && !string.IsNullOrEmpty(_searchTerm)
            //            && engagement.Description.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase);
            return engagement => engagement.ClientId == _clientId
                && engagement.TenantId == _tenantId
                && (string.IsNullOrEmpty(_searchTerm) || (engagement.Description != null && engagement.Description.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
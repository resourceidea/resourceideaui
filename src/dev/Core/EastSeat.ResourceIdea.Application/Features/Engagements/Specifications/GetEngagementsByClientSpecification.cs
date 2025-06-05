using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Specifications;

/// <summary>
/// Specification to retrieve an engagement by the owning client.
/// </summary>
/// <param name="clientId">Client identifier.</param>
/// <param name="searchTerm">Optional search term to filter engagements.</param>
public class GetEngagementsByClientSpecification(ClientId clientId, string? searchTerm = null) : BaseSpecification<Engagement>
{
    private readonly ClientId _clientId = clientId;
    private readonly string? _searchTerm = searchTerm;

    /// <summary>
    /// Criteria to retrieve an engagements by its owning client.
    /// </summary>
    public override Expression<Func<Engagement, bool>> Criteria => engagement => 
        engagement.ClientId == _clientId &&
        (string.IsNullOrEmpty(_searchTerm) || 
         engagement.Description.Contains(_searchTerm));
}
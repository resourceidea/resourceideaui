using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Clients.Entities;

using System.Linq.Expressions;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Specifications;

/// <summary>
/// Specification to query using client name.
/// </summary>
/// <param name="filters">List of filters used to query clients.</param>
public sealed class ClientNameSpecification(Dictionary<string, string> filters) : BaseStringSpecification<Client>(filters)
{
    protected override Expression<Func<Client, bool>> GetExpression() => client => client.Name.Contains(_filters[GetFilterKey()]);

    protected override string GetFilterKey() => "name";
}

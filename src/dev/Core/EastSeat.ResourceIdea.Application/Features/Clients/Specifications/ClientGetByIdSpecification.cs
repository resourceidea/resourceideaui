using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

using System.Linq.Expressions;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Specifications;

/// <summary>
/// Specification to get a client by DepartmentId.
/// </summary>
public sealed class ClientGetByIdSpecification(ClientId clientId, TenantId tenantId) : BaseSpecification<Client>
{
    private readonly ClientId _clientId = clientId;
    private readonly TenantId _tenantId = tenantId;

    public override Expression<Func<Client, bool>> Criteria =>
        client => client.Id == _clientId && client.TenantId == _tenantId;
}
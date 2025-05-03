// =============================================================================================================
// File: TenantClientsSpecification.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Clients\Specifications\TenantClientsSpecification.cs
// Description: Specification for retrieving tenant clients
// =============================================================================================================

using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Specifications;

/// <summary>
/// Specification for retrieving tenant clients.
/// </summary>
/// <param name="tenantId"></param>
public class TenantClientsSpecification(TenantId tenantId) : BaseSpecification<Client>
{
    public TenantId TenantId => tenantId;

    public override Expression<Func<Client, bool>> Criteria =>
        client => client.TenantId == tenantId;
}

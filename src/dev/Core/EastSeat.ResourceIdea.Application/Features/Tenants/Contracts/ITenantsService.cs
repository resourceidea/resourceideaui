using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;

/// <summary>
/// Tenants service interface.
/// </summary>
public interface ITenantsService : IDataStoreService<Tenant>
{
    /// <summary>
    /// Get the tenant ID from the login session.
    /// </summary>
    /// <returns></returns>
    Guid GetTenantIdFromLoginSession(CancellationToken cancellationToken);
}

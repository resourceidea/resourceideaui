using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.DataStore.Services;

/// <summary>
/// Service for performing CRUD operations on Tenant entities.
/// </summary>
public sealed class TenantsService : ITenantsService
{
    /// <summary>
    /// Adds a new Tenant entity to the data store.
    /// </summary>
    /// <param name="entity">The Tenant entity to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{T}"/>.</returns>
    public Task<ResourceIdeaResponse<Tenant>> AddAsync(Tenant entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Deletes a Tenant entity from the data store.
    /// </summary>
    /// <param name="entity">The Tenant entity to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{T}"/>.</returns>
    public Task<ResourceIdeaResponse<Tenant>> DeleteAsync(Tenant entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves a Tenant entity from the data store based on the specified specification.
    /// </summary>
    /// <param name="specification">The specification to filter the Tenant entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{T}"/>.</returns>
    public Task<ResourceIdeaResponse<Tenant>> GetByIdAsync(BaseSpecification<Tenant> specification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves a paged list of Tenant entities from the data store based on the specified page, size, and optional specification.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="size">The number of items per page.</param>
    /// <param name="specification">The optional specification to filter the Tenant entities.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{PagedListResponse{T}}"/>.</returns>
    public Task<ResourceIdeaResponse<PagedListResponse<Tenant>>> GetPagedListAsync(int page, int size, BaseSpecification<Tenant>? specification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Updates an existing Tenant entity in the data store.
    /// </summary>
    /// <param name="entity">The Tenant entity to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{T}"/>.</returns>
    public Task<ResourceIdeaResponse<Tenant>> UpdateAsync(Tenant entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public TenantId GetTenantIdFromLoginSession(CancellationToken cancellationToken)
    {
        // TODO: Implement logic that will read the tenant ID from the login session.
        return TenantId.Create("841C6122-59E8-4294-93B8-D21C0BEB6724");
    }
}

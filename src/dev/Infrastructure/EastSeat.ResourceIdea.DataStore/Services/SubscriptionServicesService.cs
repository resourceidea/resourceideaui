using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Contracts;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.DataStore.Services;

/// <summary>
/// Service for performing CRUD operations on SubscriptionService entities.
/// </summary>
public sealed class SubscriptionServicesService : ISubscriptionServicesService
{
    /// <summary>
    /// Adds a new SubscriptionService entity to the data store.
    /// </summary>
    /// <param name="entity">The SubscriptionService entity to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{SubscriptionService}"/>.</returns>
    public Task<ResourceIdeaResponse<SubscriptionService>> AddAsync(SubscriptionService entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Deletes a SubscriptionService entity from the data store.
    /// </summary>
    /// <param name="entity">The SubscriptionService entity to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{SubscriptionService}"/>.</returns>
    public Task<ResourceIdeaResponse<SubscriptionService>> DeleteAsync(SubscriptionService entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves a SubscriptionService entity from the data store based on the specified specification.
    /// </summary>
    /// <param name="specification">The specification to filter the entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{SubscriptionService}"/>.</returns>
    public Task<ResourceIdeaResponse<SubscriptionService>> GetByIdAsync(BaseSpecification<SubscriptionService> specification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves a paged list of SubscriptionService entities from the data store based on the specified page, size, and optional specification.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="size">The number of items per page.</param>
    /// <param name="specification">The optional specification to filter the entities.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{PagedListResponse{SubscriptionService}}"/>.</returns>
    public Task<ResourceIdeaResponse<PagedListResponse<SubscriptionService>>> GetPagedListAsync(int page, int size, BaseSpecification<SubscriptionService>? specification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Updates an existing SubscriptionService entity in the data store.
    /// </summary>
    /// <param name="entity">The SubscriptionService entity to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{SubscriptionService}"/>.</returns>
    public Task<ResourceIdeaResponse<SubscriptionService>> UpdateAsync(SubscriptionService entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

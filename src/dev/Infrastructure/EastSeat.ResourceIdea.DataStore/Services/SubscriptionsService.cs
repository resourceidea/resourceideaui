using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Contracts;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.DataStore.Services;

/// <summary>
/// Service for performing CRUD operations on subscriptions.
/// </summary>
public sealed class SubscriptionsService : ISubscriptionsService
{
    /// <summary>
    /// Adds a new subscription to the data store.
    /// </summary>
    /// <param name="entity">The subscription entity to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a response containing the added subscription.</returns>
    public Task<ResourceIdeaResponse<Subscription>> AddAsync(Subscription entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Deletes a subscription from the data store.
    /// </summary>
    /// <param name="entity">The subscription entity to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a response containing the deleted subscription.</returns>
    public Task<ResourceIdeaResponse<Subscription>> DeleteAsync(Subscription entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves a subscription from the data store based on the specified specification.
    /// </summary>
    /// <param name="specification">The specification to filter the subscription.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a response containing the retrieved subscription.</returns>
    public Task<ResourceIdeaResponse<Subscription>> GetByIdAsync(BaseSpecification<Subscription> specification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves a paged list of subscriptions from the data store based on the specified page, size, and optional specification.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="size">The number of items per page.</param>
    /// <param name="specification">The optional specification to filter the subscriptions.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a response containing the paged list of subscriptions.</returns>
    public Task<ResourceIdeaResponse<PagedListResponse<Subscription>>> GetPagedListAsync(int page, int size, BaseSpecification<Subscription>? specification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Updates an existing subscription in the data store.
    /// </summary>
    /// <param name="entity">The subscription entity to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a response containing the updated subscription.</returns>
    public Task<ResourceIdeaResponse<Subscription>> UpdateAsync(Subscription entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

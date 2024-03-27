using System.Linq.Expressions;

using EastSeat.ResourceIdea.Domain.Entities;
using EastSeat.ResourceIdea.Domain.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Contracts.Persistence;

/// <summary>
/// Subscription repository.
/// </summary>
public interface ISubscriptionRepository
{
    /// <summary>
    /// Get entity by ID.
    /// </summary>
    /// <param name="id">Entity ID.</param>
    /// <returns>Gets entity whose ID has been identified; Otherwise, null.</returns>
    Task<Subscription?> GetByIdAsync(Guid id);

    /// <summary>
    /// List all entities.
    /// </summary>
    /// <returns>Readonly list of entities.</returns>
    Task<IReadOnlyList<Subscription>> ListAllAsync();

    /// <summary>
    /// Add entity.
    /// </summary>
    /// <param name="entity">Entity to be added.</param>
    /// <returns>Entity that has been added.</returns>
    Task<Subscription> AddAsync(Subscription model);

    /// <summary>
    /// Update entity.
    /// </summary>
    /// <returns>Entity that has been updated.</returns>
    Task<Subscription> UpdateAsync(Subscription model);

    /// <summary>
    /// Delete entity.
    /// </summary>
    /// <param name="id">ID of the entity to be deleted.</param>
    /// <returns>Delete task result.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Get a paginated readonly list of entities.
    /// </summary>
    /// <param name="page">Page of the list to be returned.</param>
    /// <param name="size">Size of the page to be returned.</param>
    /// <param name="filter">Query filter.</param>
    /// <returns>Readonly paged list of entities.</returns>
    Task<PagedList<Subscription>> GetPagedListAsync(int page, int size, Expression<Func<Subscription, bool>>? filter = null);

    /// <summary>
    /// Check if the subscriber name is already in user.
    /// </summary>
    /// <param name="subscriberName">Subscriber name to check.</param>
    /// <returns>True if name if already in use; otherwise False.</returns>
    Task<bool> IsSubscriberNameAlreadyInUse(string? subscriberName);
}

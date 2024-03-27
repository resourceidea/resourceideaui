using System.Linq.Expressions;

using EastSeat.ResourceIdea.Domain.Entities;
using EastSeat.ResourceIdea.Domain.ValueObjects;

using Optional;

namespace EastSeat.ResourceIdea.Application.Contracts.Persistence;

/// <summary>
/// Client repository.
/// </summary>
public interface IClientRepository
{
    /// <summary>
    /// Get entity by ID.
    /// </summary>
    /// <param name="id">Entity ID.</param>
    /// <returns>Gets entity whose ID has been identified; Otherwise, null.</returns>
    Task<Option<Client>> GetByIdAsync(Guid id);

    /// <summary>
    /// List all entities.
    /// </summary>
    /// <returns>Readonly list of entities.</returns>
    Task<Option<IReadOnlyList<Client>>> ListAllAsync();

    /// <summary>
    /// Add entity.
    /// </summary>
    /// <param name="entity">Entity to be added.</param>
    /// <returns>Entity that has been added.</returns>
    Task<Option<Client>> AddAsync(Client client);

    /// <summary>
    /// Update entity.
    /// </summary>
    /// <returns>Entity that has been updated.</returns>
    Task<Option<Client>> UpdateAsync(Client client);

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
    Task<PagedList<Client>> GetPagedListAsync(int page, int size, Expression<Func<Client, bool>>? filter = null);
}

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Common.Entities;

namespace EastSeat.ResourceIdea.Application.Features.Common.Contracts;

/// <summary>
/// Base repository.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IAsyncRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Get a paginated readonly list of entities.
    /// </summary>
    /// <param name="page">Page of the list to be returned.</param>
    /// <param name="size">Size of the page to be returned.</param>
    /// <param name="filter">Query filter.</param>
    /// <returns>Readonly paged list of entities.</returns>
    Task<PagedListResponse<T>> GetPagedListAsync(
        int page,
        int size,
        BaseSpecification<T> specification,
        CancellationToken cancellationToken);

    /// <summary>
    /// Get entity by SubscriptionId.
    /// </summary>
    /// <param name="specification">Entity SubscriptionId.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Entity.</returns>
    Task<Optional<T>> GetByIdAsync(BaseSpecification<T> specification, CancellationToken cancellationToken);

    /// <summary>
    /// Add entity.
    /// </summary>
    /// <param name="entity">Entity to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Entity.</returns>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken);

    /// <summary>
    /// Update entity.
    /// </summary>
    /// <param name="entity">Entity to update.</param>
    /// <returns>Entity.</returns>
    Task<Optional<T>> UpdateAsync(T entity, CancellationToken cancellationToken);

    /// <summary>
    /// Delete entity.
    /// </summary>
    /// <param name="entity">Entity to delete.</param>
    /// <returns>Entity.</returns>
    Task<Optional<T>> DeleteAsync(T entity);
}
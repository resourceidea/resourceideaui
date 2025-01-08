using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Common.Contracts;

/// <summary>
/// Represents a generic data store service for performing CRUD operations on entities.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface IDataStoreService<T> where T : BaseEntity
{
    /// <summary>
    /// Retrieves a paged list of entities from the data store based on the specified page and size.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="size">The number of items per page.</param>
    /// <param name="specification">The optional specification to filter the entities.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{PagedListResponse{T}}"/>.</returns>
    Task<ResourceIdeaResponse<PagedListResponse<T>>> GetPagedListAsync(
        int page,
        int size,
        Optional<BaseSpecification<T>> specification,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves an entity from the data store based on the specified specification.
    /// </summary>
    /// <param name="specification">The specification to filter the entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{T}"/>.</returns>
    Task<ResourceIdeaResponse<T>> GetByIdAsync(
        BaseSpecification<T> specification,
        CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new entity to the data store.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{T}"/>.</returns>
    Task<ResourceIdeaResponse<T>> AddAsync(T entity, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing entity in the data store.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{T}"/>.</returns>
    Task<ResourceIdeaResponse<T>> UpdateAsync(T entity, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes an entity from the data store.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{T}"/>.</returns>
    Task<ResourceIdeaResponse<T>> DeleteAsync(T entity, CancellationToken cancellationToken);
}

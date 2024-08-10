using EastSeat.ResourceIdea.Domain.Common.Entities;

namespace EastSeat.ResourceIdea.Application.Features.Common.Contracts;

/// <summary>
/// Represents a unit of work.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Saves the changes made to the data store.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Begins a new transaction.
    /// </summary>
    void BeginTransaction();

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    void Commit();

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    void Rollback();

    /// <summary>
    /// Gets the repository for the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the repository.</typeparam>
    /// <returns>The repository for the specified type.</returns>
    IAsyncRepository<T> GetRepository<T>() where T : BaseEntity;
}
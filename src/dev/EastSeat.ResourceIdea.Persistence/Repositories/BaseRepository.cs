using System.Linq.Expressions;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Domain.Common;
using EastSeat.ResourceIdea.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;

using Optional;

namespace EastSeat.ResourceIdea.Persistence.Repositories;

/// <summary>
/// Base repository.
/// </summary>
/// <remarks>
/// Initializes <see cref="BaseRepository{T}" />.
/// </remarks>
/// <param name="dbContext">App database context.</param>
public class BaseRepository<T>(ResourceIdeaDbContext dbContext) : IAsyncRepository<T> where T : BaseSubscriptionEntity
{
    protected readonly ResourceIdeaDbContext dbContext = dbContext;

    /// <inheritdoc />
    public async Task<T> AddAsync(T entity)
    {
        await dbContext.Set<T>().AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return entity;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var entity = await dbContext.Set<T>().FindAsync(id);
        if (entity is not null)
        {
            entity.IsDeleted = true;
            await UpdateAsync(entity);
        }
    }

    /// <inheritdoc />
    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        T? t = await dbContext.Set<T>().FindAsync(id);
        return t;
    }

    /// <inheritdoc />
    public async Task<PagedList<T>> GetPagedListAsync(int page, int size, Expression<Func<T, bool>> specification)
    {
        var query = dbContext.Set<T>().Where(specification)
                                      .Skip(size * (page - 1))
                                      .Take(size);

        var items = await query.ToListAsync();
        var totalItemsCount = await dbContext.Set<T>().CountAsync(specification);

        var pagedList = new PagedList<T>
        {
            TotalCount = totalItemsCount,
            Items = Option.Some<IReadOnlyList<T>>(items),
            CurrentPage = page,
            PageSize = size
        };

        return pagedList;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await dbContext.Set<T>().ToListAsync() ?? Enumerable.Empty<T>().ToList();
    }

    /// <inheritdoc />
    public async Task<T> UpdateAsync(T entity)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();

        return entity;
    }
}

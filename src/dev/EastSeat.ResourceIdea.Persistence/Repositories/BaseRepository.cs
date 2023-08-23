using EastSeat.ResourceIdea.Application.Contracts.Persistence;

using Microsoft.EntityFrameworkCore;

namespace EastSeat.ResourceIdea.Persistence.Repositories;

/// <summary>
/// Base repository.
/// </summary>
public class BaseRepository<T> : IAsyncRepository<T> where T : class
{
    protected readonly ResourceIdeaDbContext dbContext;

    /// <summary>
    /// Initializes <see cref="BaseRepository{T}" />.
    /// </summary>
    /// <param name="dbContext">App database context.</param>
    public BaseRepository(ResourceIdeaDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

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
            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        T? t = await dbContext.Set<T>().FindAsync(id);
        return t;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<T>> GetPaginatedListAsync(int page, int size)
    {
        return await dbContext.Set<T>().Skip(size * (page - 1)).Take(size).ToListAsync() ?? Enumerable.Empty<T>().ToList();
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

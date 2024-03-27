using System.Linq.Expressions;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Domain.Entities;
using EastSeat.ResourceIdea.Domain.ValueObjects;
using EastSeat.ResourceIdea.Persistence.Extensions;

using Microsoft.EntityFrameworkCore;

using Optional;

namespace EastSeat.ResourceIdea.Persistence.Repositories;

/// <summary>
/// Repository for the client records.
/// </summary>
public class ClientRepository(ResourceIdeaDbContext dbContext) : IClientRepository
{
    private readonly ResourceIdeaDbContext dbContext = dbContext;

    public async Task<Option<Client>> AddAsync(Client entity)
    {
        var clientEntityToAdd = entity.MapToEntity();

        try
        {
            var entityEntry = await dbContext.Clients.AddAsync(clientEntityToAdd);
            await dbContext.SaveChangesAsync();

            return Option.Some(entityEntry.Entity.MapToModel());
        }
        catch (DbUpdateException)
        {
            // TODO: Log failure to add entity.
            return Option.None<Client>();
        }
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Client?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<PagedList<Client>> GetPagedListAsync(int page, int size, Expression<Func<Client, bool>>? filter = null)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Client>> ListAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Client> UpdateAsync(Client entity)
    {
        throw new NotImplementedException();
    }

    Task<Option<Client>> IClientRepository.GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    Task<Option<IReadOnlyList<Client>>> IClientRepository.ListAllAsync()
    {
        throw new NotImplementedException();
    }

    Task<Option<Client>> IClientRepository.UpdateAsync(Client client)
    {
        throw new NotImplementedException();
    }
}

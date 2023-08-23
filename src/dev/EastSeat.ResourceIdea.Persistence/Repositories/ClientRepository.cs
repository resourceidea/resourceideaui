using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Persistence.Repositories;

/// <summary>
/// Repository for the client records.
/// </summary>
public class ClientRepository : BaseRepository<Client>, IClientRepository
{
    public ClientRepository(ResourceIdeaDbContext dbContext) : base(dbContext)
    {
    }
}

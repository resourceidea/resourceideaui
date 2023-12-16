using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Persistence.Repositories;

/// <summary>
/// Repository for the client records.
/// </summary>
public class ClientRepository(ResourceIdeaDbContext dbContext) : BaseRepository<Client>(dbContext), IClientRepository
{
}

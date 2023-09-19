using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Application.Contracts.Persistence;

/// <summary>
/// Client repository.
/// </summary>
public interface IClientRepository : IAsyncRepository<Client>
{
}

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Clients.Entities;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Contracts;

/// <summary>
/// Client repository interface.
/// </summary>
public interface IClientRepository : IAsyncRepository<Client> {}
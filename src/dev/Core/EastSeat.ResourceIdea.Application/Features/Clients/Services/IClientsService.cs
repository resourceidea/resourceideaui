using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Services;

/// <summary>
/// Service for managing and handling operations clients.
/// </summary>
public interface IClientsService
{
    /// <summary>
    /// Create a new client.
    /// </summary>
    /// <param name="client">Client to be created.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    Task<ResourceIdeaResponse<ClientModel>> CreateClientAsync(
        Client client,
        CancellationToken cancellationToken);

    /// <summary>
    /// Update a client.
    /// </summary>
    /// <param name="client">Client update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Update response.</returns>
    Task<ResourceIdeaResponse<ClientModel>> UpdateClientAsync(
        Client client,
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Get a client by id.
    /// </summary>
    /// <param name="specification">Provides filter for client query.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Client get response.</returns>
    Task<ResourceIdeaResponse<ClientModel>> GetByIdAsync(
        BaseSpecification<Client> specification,
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Get a paged list of clients.
    /// </summary>
    /// <returns>Clients list get response..</returns>
    Task<ResourceIdeaResponse<PagedListResponse<ClientModel>>> GetPagedListAsync(
        int page,
        int pageSize,
        BaseSpecification<Client> specification,
        CancellationToken cancellationToken);
}
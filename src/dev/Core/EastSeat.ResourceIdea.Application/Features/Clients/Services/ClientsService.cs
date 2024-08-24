using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Services;

/// <summary>
/// Service for managing and handling operations clients.
/// </summary>
public class ClientsService(IClientRepository clientRepository, IMapper mapper) : IClientsService
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IMapper _mapper = mapper;

    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<ClientModel>> CreateClientAsync(
        Client client,
        CancellationToken cancellationToken)
    {
        var createClientResult = await _clientRepository.AddAsync(client, cancellationToken);
        if (createClientResult.IsFailure)
        {
            return ResourceIdeaResponse<ClientModel>.Failure(createClientResult.Error);
        }

        return ResourceIdeaResponse<ClientModel>.Success(
            Optional<ClientModel>.Some(_mapper.Map<ClientModel>(createClientResult.Content.Value)));
    }

    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<ClientModel>> GetByIdAsync(
        BaseSpecification<Client> specification,
        CancellationToken cancellationToken)
    {
        var clientQueryResponse = await _clientRepository.GetByIdAsync(specification, cancellationToken);
        if (clientQueryResponse.IsFailure)
        {
            return ResourceIdeaResponse<ClientModel>.Failure(clientQueryResponse.Error);
        }

        return ResourceIdeaResponse<ClientModel>.Success(
            Optional<ClientModel>.Some(_mapper.Map<ClientModel>(clientQueryResponse.Content.Value)));
    }

    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<PagedListResponse<ClientModel>>> GetPagedListAsync(
        int page,
        int pageSize,
        BaseSpecification<Client> specification,
        CancellationToken cancellationToken)
    {
        var clients = await _clientRepository.GetPagedListAsync(page, pageSize, specification, cancellationToken);
        return ResourceIdeaResponse<PagedListResponse<ClientModel>>
                    .Success(Optional<PagedListResponse<ClientModel>>.Some(_mapper.Map<PagedListResponse<ClientModel>>(clients)));
    }

    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<ClientModel>> UpdateClientAsync(
        Client client,
        CancellationToken cancellationToken)
    {

        var updatedClient = await _clientRepository.UpdateAsync(client, cancellationToken);

        return ResourceIdeaResponse<ClientModel>.Success(
            Optional<ClientModel>.Some(_mapper.Map<ClientModel>(updatedClient)));
    }
}
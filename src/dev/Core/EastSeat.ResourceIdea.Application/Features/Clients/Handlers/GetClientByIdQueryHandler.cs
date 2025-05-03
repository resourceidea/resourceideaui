using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Application.Features.Clients.Specifications;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

/// <summary>
/// Handles the query to get a client by id.
/// </summary>
/// <param name="clientsService"></param>
public sealed class GetClientByIdQueryHandler(IClientsService clientsService)
    : IRequestHandler<GetClientByIdQuery, ResourceIdeaResponse<ClientModel>>
{
    private readonly IClientsService _clientsService = clientsService;

    public async Task<ResourceIdeaResponse<ClientModel>> Handle(
        GetClientByIdQuery request,
        CancellationToken cancellationToken)
    {
        var getClientByIdSpecification = new ClientGetByIdSpecification(request.ClientId);
        var result = await _clientsService.GetByIdAsync(getClientByIdSpecification, cancellationToken);

        if (result.IsFailure)
        {
            return ResourceIdeaResponse<ClientModel>.Failure(result.Error);
        }

        if (result.Content.HasValue is false)
        {
            return ResourceIdeaResponse<ClientModel>.NotFound();
        }

        return result.Content.Value.ToResourceIdeaResponse<Client, ClientModel>();
    }
}
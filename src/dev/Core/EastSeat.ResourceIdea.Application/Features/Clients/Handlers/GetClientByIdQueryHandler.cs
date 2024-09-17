using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Application.Features.Clients.Services;
using EastSeat.ResourceIdea.Application.Features.Clients.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

public sealed class GetClientByIdQueryHandler(
    IClientsService clientsService) : IRequestHandler<GetClientByIdQuery, ResourceIdeaResponse<ClientModel>>
{
    private readonly IClientsService _clientsService = clientsService;

    public async Task<ResourceIdeaResponse<ClientModel>> Handle(
        GetClientByIdQuery request,
        CancellationToken cancellationToken)
    {
        var getClientByIdSpecification = new ClientGetByIdSpecification(request.ClientId);
        return await _clientsService.GetByIdAsync(getClientByIdSpecification, cancellationToken);
    }
}
using AutoMapper;

using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Application.Features.Clients.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

public sealed class GetClientByIdQueryHandler(
    IClientsService clientsService,
    IMapper mapper) : IRequestHandler<GetClientByIdQuery, ResourceIdeaResponse<ClientModel>>
{
    private readonly IClientsService _clientsService = clientsService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<ClientModel>> Handle(
        GetClientByIdQuery request,
        CancellationToken cancellationToken)
    {
        var getClientByIdSpecification = new ClientGetByIdSpecification(request.ClientId);
        var result = await _clientsService.GetByIdAsync(getClientByIdSpecification, cancellationToken);

        // TODO: Remove IsFailure check because it is redundant.
        return _mapper.Map<ResourceIdeaResponse<ClientModel>>(result);
    }
}
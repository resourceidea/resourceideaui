using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Application.Features.Clients.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

public sealed class GetClientByIdQueryHandler(IAsyncRepository<Client> clientRepository, IMapper mapper)
    : IRequestHandler<GetClientByIdQuery, ResourceIdeaResponse<ClientModel>>
{
    private readonly IAsyncRepository<Client> _clientRepository = clientRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<ClientModel>> Handle(
        GetClientByIdQuery request,
        CancellationToken cancellationToken)
    {
        var getClientByIdSpecification = new ClientGetByIdSpecification(request.ClientId);
        Optional<Client> clientQuery = await _clientRepository.GetByIdAsync(getClientByIdSpecification, cancellationToken);

        Client client = clientQuery.Match(
            some: client => client,
            none: () => EmptyClient.Instance);

        if (client.IsEmpty())
        {
            return ResourceIdeaResponse<ClientModel>.NotFound();
        }

        return new ResourceIdeaResponse<ClientModel>
        {
            Success = true,
            Content = Optional<ClientModel>.Some(_mapper.Map<ClientModel>(client))
        };
    }
}
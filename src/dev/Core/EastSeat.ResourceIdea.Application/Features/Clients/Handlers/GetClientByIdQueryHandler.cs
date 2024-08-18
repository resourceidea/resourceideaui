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
        var getClientQueryResult = await _clientRepository.GetByIdAsync(getClientByIdSpecification, cancellationToken);
        if (getClientQueryResult.IsFailure)
        {
            return ResourceIdeaResponse<ClientModel>.Failure(getClientQueryResult.Error);
        }

        return ResourceIdeaResponse<ClientModel>.Success(Optional<ClientModel>.Some(_mapper.Map<ClientModel>(getClientQueryResult.Content.Value)));
    }
}
using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Client.DTO;
using EastSeat.ResourceIdea.Application.Features.Client.Queries;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Client.Handlers;

public class GetClientsListQueryHandler (IMapper mapper, IAsyncRepository<Domain.Entities.Client> clientRepository) : IRequestHandler<GetClientsListQuery, IReadOnlyList<ClientListDTO>>
{

    public async Task<IReadOnlyList<ClientListDTO>> Handle(GetClientsListQuery request, CancellationToken cancellationToken)
    {
        var clients = await clientRepository.ListAllAsync();
        return mapper.Map<IReadOnlyList<ClientListDTO>>(clients.OrderBy(c => c?.Name));
    }
}

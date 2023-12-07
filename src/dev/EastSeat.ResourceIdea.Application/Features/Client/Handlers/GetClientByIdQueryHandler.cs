using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Client.DTO;
using EastSeat.ResourceIdea.Application.Features.Client.Queries;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Client.Handlers;

public class GetClientByIdQueryHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Client> clientRepository) : IRequestHandler<GetClientByIdQuery, ClientDTO>
{
    public async Task<ClientDTO> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetByIdAsync(request.Id);
        return mapper.Map<ClientDTO>(client);
    }
}

using EastSeat.ResourceIdea.Application.Features.Client.DTO;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Client.Queries;

public class GetClientsListQuery : IRequest<IReadOnlyList<ClientListDTO>>
{
}

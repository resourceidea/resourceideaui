using EastSeat.ResourceIdea.Application.Features.Client.DTO;
using EastSeat.ResourceIdea.Domain.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Client.Queries;

public class GetClientsListQuery : IRequest<PagedList<ClientListDTO>>
{
    /// <summary>Clients list page number.</summary>
    public int Page { get; set; } = 1;

    /// <summary>Clients list page size.</summary>
    public int Size { get; set; } = 10;

    /// <summary>Query filter.</summary>
    public string Filter { get; set; } = string.Empty;
}

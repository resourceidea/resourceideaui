using System.Linq.Expressions;

using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Clients.DTO;
using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Domain.ValueObjects;

using MediatR;

using Optional;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

public class GetClientsListQueryHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Client> clientRepository) : IRequestHandler<GetClientsListQuery, PagedList<ClientListDTO>>
{

    public async Task<PagedList<ClientListDTO>> Handle(GetClientsListQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Domain.Entities.Client, bool>>? filter = null;
        if (!string.IsNullOrEmpty(request.Filter))
        {
            filter = (Domain.Entities.Client client) => client.Name.Contains(request.Filter);
        }

        var pagedList = await clientRepository.GetPagedListAsync(request.Page, request.Size, filter);

        return new PagedList<ClientListDTO>
        {
            Items = Option.Some(mapper.Map<IReadOnlyList<ClientListDTO>>(pagedList.Items.ValueOr([]))),
            TotalCount = pagedList.TotalCount,
            PageSize = pagedList.PageSize,
            CurrentPage = pagedList.CurrentPage
        };
    }
}

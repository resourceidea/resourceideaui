using System.Linq.Expressions;

using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Client.DTO;
using EastSeat.ResourceIdea.Application.Features.Client.Queries;
using EastSeat.ResourceIdea.Domain.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Client.Handlers;

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
            Items = mapper.Map<IReadOnlyList<ClientListDTO>>(pagedList.Items),
            TotalCount = pagedList.TotalCount,
            PageSize = pagedList.PageSize,
            CurrentPage = pagedList.CurrentPage
        };
    }
}

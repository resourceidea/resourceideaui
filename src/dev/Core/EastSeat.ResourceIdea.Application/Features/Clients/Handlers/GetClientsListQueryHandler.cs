using AutoMapper;

using EastSeat.ResourceIdea.Application.Extensions;
using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Application.Features.Clients.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

public sealed class GetClientsListQueryHandler(
    IAsyncRepository<Client> clientRepository,
    IMapper mapper) : IRequestHandler<GetClientsListQuery, ResourceIdeaResponse<PagedListResponse<ClientModel>>>
{
    private readonly IAsyncRepository<Client> _clientRepository = clientRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<PagedListResponse<ClientModel>>> Handle(GetClientsListQuery request, CancellationToken cancellationToken)
    {
        var specification = GetClientQuerySpecification(request.Filter);
        var clients = await _clientRepository.GetPagedListAsync(
            request.CurrentPageNumber,
            request.PageSize,
            specification,
            cancellationToken);

        return ResourceIdeaResponse<PagedListResponse<ClientModel>>
                    .Success(Optional<PagedListResponse<ClientModel>>.Some(_mapper.Map<PagedListResponse<ClientModel>>(clients)));
    }

    private static BaseSpecification<Client> GetClientQuerySpecification(string combinedFilters)
    {
        var filters = combinedFilters.GetFiltersAsDictionary(delimiter: [';'], keyValueSeparator: ['=']);
        return new ClientNameSpecification(filters);
    }
}

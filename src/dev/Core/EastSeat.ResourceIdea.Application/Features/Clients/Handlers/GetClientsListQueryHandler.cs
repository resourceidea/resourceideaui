using EastSeat.ResourceIdea.Application.Extensions;
using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Application.Features.Clients.Services;
using EastSeat.ResourceIdea.Application.Features.Clients.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

public sealed class GetClientsListQueryHandler(
    IClientsService clientsService)
    : IRequestHandler<GetClientsListQuery, ResourceIdeaResponse<PagedListResponse<ClientModel>>>
{
    private readonly IClientsService _clientsService = clientsService;

    public async Task<ResourceIdeaResponse<PagedListResponse<ClientModel>>> Handle(GetClientsListQuery request, CancellationToken cancellationToken)
    {
        var specification = GetClientQuerySpecification(request.Filter);
        return await _clientsService.GetPagedListAsync(
            page: request.CurrentPageNumber,
            pageSize: request.PageSize,
            specification,
            cancellationToken);
    }

    private static BaseSpecification<Client> GetClientQuerySpecification(string combinedFilters)
    {
        var filters = combinedFilters.GetFiltersAsDictionary(delimiter: [';'], keyValueSeparator: ['=']);
        return new ClientNameSpecification(filters);
    }
}

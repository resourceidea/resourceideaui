using EastSeat.ResourceIdea.Application.Extensions;
using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Application.Features.Clients.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

/// <summary>
/// Handles query to get a list of clients.
/// </summary>
/// <param name="clientsService"></param>
public sealed class GetClientsListQueryHandler(IClientsService clientsService)
    : IRequestHandler<GetClientsListQuery, ResourceIdeaResponse<PagedListResponse<ClientModel>>>
{
    private readonly IClientsService _clientsService = clientsService;

    public async Task<ResourceIdeaResponse<PagedListResponse<ClientModel>>> Handle(GetClientsListQuery request, CancellationToken cancellationToken)
    {
        var specification = GetClientQuerySpecification(request.Filter);
        var result = await _clientsService.GetPagedListAsync(
            page: request.CurrentPageNumber,
            size: request.PageSize,
            specification,
            cancellationToken);

        if (result.IsFailure)
        {
            return ResourceIdeaResponse<PagedListResponse<ClientModel>>.Failure(result.Error);
        }

        if (result.Content.HasValue is false)
        {
            return ResourceIdeaResponse<PagedListResponse<ClientModel>>.NotFound();
        }

        return result.Content.Value.ToResourceIdeaResponse();
    }

    private static BaseSpecification<Client> GetClientQuerySpecification(string combinedFilters)
    {
        var filters = combinedFilters.GetFiltersAsDictionary(delimiter: [';'], keyValueSeparator: ['=']);
        return new ClientNameSpecification(filters);
    }
}

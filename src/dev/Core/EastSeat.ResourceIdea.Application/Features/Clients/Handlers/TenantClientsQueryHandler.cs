using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Application.Features.Clients.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Handlers;

/// <summary>
/// Handles query to get a list of clients.
/// </summary>
/// <param name="clientsService"></param>
public sealed class TenantClientsQueryHandler(IClientsService clientsService)
    : BaseHandler,
      IRequestHandler<TenantClientsQuery, ResourceIdeaResponse<PagedListResponse<TenantClientModel>>>
{
    private readonly IClientsService _clientsService = clientsService;

    public async Task<ResourceIdeaResponse<PagedListResponse<TenantClientModel>>> Handle(
        TenantClientsQuery query,
        CancellationToken cancellationToken)
    {
        TenantClientsSpecification specification = new(query.TenantId);
        var queryResponse = await _clientsService.GetPagedListAsync(
            page: query.PageNumber,
            size: query.PageSize,
            specification,
            cancellationToken);

        return queryResponse.ToResourceIdeaResponse<Client, TenantClientModel>();
    }
}

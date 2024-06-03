using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Queries;

/// <summary>
/// Query to get a list of clients.
/// </summary>
public sealed class GetClientsListQuery : IRequest<ResourceIdeaResponse<PagedListResponse<ClientModel>>>
{
    public int CurrentPageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string Filter { get; set; } = string.Empty;
}

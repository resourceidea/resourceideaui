using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Queries;

public sealed class GetAllEngagementsQuery (
    int pageNumber,
    int pageSize) : IRequest<ResourceIdeaResponse<PagedListResponse<EngagementModel>>>
{
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
}

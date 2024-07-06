using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Queries;

public sealed class GetEngagementsByStatusQuery (
    int pageNumber,
    int pageSize) : IRequest<ResourceIdeaResponse<PagedListResponse<EngagementModel>>>
{
    /// <summary>
    /// Page number.
    /// </summary>
    public int PageNumber { get; } = pageNumber;

    /// <summary>
    /// Page size.
    /// </summary>
    public int PageSize { get; } = pageSize;

    /// <summary>
    /// The status to retrieve engagements for.
    /// </summary>
    public EngagementStatus EngagementStatus { get; set; }
}

﻿using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Queries;

/// <summary>
/// Query to retrieve engagements by client.
/// </summary>
public sealed class GetEngagementsByClientQuery (
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
    /// The client identifier to retrieve engagements for.
    /// </summary>
    public ClientId ClientId { get; init; }

    /// <summary>
    /// Optional search term to filter engagements.
    /// </summary>
    public string? SearchTerm { get; init; }

    /// <summary>
    /// Optional sort field.
    /// </summary>
    public string? SortField { get; init; }

    /// <summary>
    /// Optional sort direction (asc or desc).
    /// </summary>
    public string? SortDirection { get; init; }
}

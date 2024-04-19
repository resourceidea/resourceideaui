namespace EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;

/// <summary>
/// Represents a request for a paged list.
/// </summary>
public sealed record PagedListRequest
{
    /// <summary>Number of the paged list being requested.</summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>Size of the paged list.</summary>
    public int PageSize { get; set; } = 10;

    /// <summary>Filter on the paged list request.</summary>
    public string Filter { get; set; } = string.Empty;
}

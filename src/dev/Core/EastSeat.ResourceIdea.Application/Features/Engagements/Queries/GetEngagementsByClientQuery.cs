using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

using System.Linq;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Queries;

/// <summary>
/// Query to retrieve engagements by client.
/// </summary>
public sealed class GetEngagementsByClientQuery(
    int pageNumber,
    int pageSize) : BaseRequest<PagedListResponse<EngagementModel>>
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
    public override ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            ClientId.ValidateRequired(),
            ValidatePageNumber(),
            ValidatePageSize(),
            ValidateSortDirection(),
            ValidateSortField(),
            ValidateSearchTerm()
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }

    /// <summary>
    /// Validates the page number parameter.
    /// </summary>
    /// <returns>Empty string if valid, otherwise an error message.</returns>
    private string ValidatePageNumber()
    {
        return PageNumber < 1
            ? "Page number must be greater than or equal to 1."
            : string.Empty;
    }

    /// <summary>
    /// Validates the page size parameter.
    /// </summary>
    /// <returns>Empty string if valid, otherwise an error message.</returns>
    private string ValidatePageSize()
    {
        if (PageSize < 1)
            return "Page size must be greater than or equal to 1.";

        if (PageSize > 100)
            return "Page size cannot exceed 100 items.";

        return string.Empty;
    }

    /// <summary>
    /// Validates the sort direction parameter.
    /// </summary>
    /// <returns>Empty string if valid, otherwise an error message.</returns>
    private string ValidateSortDirection()
    {
        if (string.IsNullOrWhiteSpace(SortDirection))
            return string.Empty;

        var validSortDirections = new[] { "asc", "desc" };
        return !validSortDirections.Contains(SortDirection.ToLowerInvariant())
            ? "Sort direction must be either 'asc' or 'desc'."
            : string.Empty;
    }

    /// <summary>
    /// Validates the sort field parameter.
    /// </summary>
    /// <returns>Empty string if valid, otherwise an error message.</returns>
    private string ValidateSortField()
    {
        if (string.IsNullOrWhiteSpace(SortField))
            return string.Empty;

        var validSortFields = new[] { "name", "startdate", "enddate", "status", "createdat" };
        return !validSortFields.Contains(SortField.ToLowerInvariant())
            ? $"Sort field must be one of: {string.Join(", ", validSortFields)}."
            : string.Empty;
    }

    /// <summary>
    /// Validates the search term parameter.
    /// </summary>
    /// <returns>Empty string if valid, otherwise an error message.</returns>
    private string ValidateSearchTerm()
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
            return string.Empty;

        return SearchTerm.Length > 100
            ? "Search term cannot exceed 100 characters."
            : string.Empty;
    }
}

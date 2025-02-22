using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Departments.Models;

namespace EastSeat.ResourceIdea.Application.Features.Departments.Queries;

/// <summary>
/// Query to get all departments with pagination and optional filtering.
/// </summary>
public sealed class GetAllDepartmentsQuery : BaseRequest<PagedListResponse<DepartmentModel>>
{
    /// <summary>
    /// Gets or sets the page number for pagination.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size for pagination.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the filter string to filter the departments.
    /// </summary>
    public string Filter { get; set; } = string.Empty;
}

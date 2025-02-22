// -----------------------------------------------------------------------------
// File: GetJobPositionsByDepartmentIdQuery.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\JobPositions\Queries\GetJobPositionsByDepartmentIdQuery.cs
// Description: Query to get job positions by department ID.
// -----------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;

namespace EastSeat.ResourceIdea.Application.Features.JobPositions.Queries;

/// <summary>
/// Query to get job positions by department ID.
/// </summary>
public sealed class GetJobPositionsByDepartmentIdQuery : BaseRequest<PagedListResponse<JobPositionModel>>
{
    /// <summary>Department identifier to filter job positions.</summary>
    public DepartmentId DepartmentId { get; set; }

    /// <summary>Gets or sets the page number for pagination.</summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>Gets or sets the page size for pagination.</summary>
    public int PageSize { get; set; } = 10;

    /// <summary>Gets or sets the filter string to filter the departments.</summary>
    public string Filter { get; set; } = string.Empty;
}
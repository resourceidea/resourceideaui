// =============================================================================
// File: GetPotentialManagersQuery.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Queries\GetPotentialManagersQuery.cs
// Description: Query to get potential managers for an employee.
// =============================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Queries;

/// <summary>
/// Query to get potential managers for an employee.
/// Excludes the current employee to prevent self-assignment.
/// </summary>
public sealed class GetPotentialManagersQuery : BaseRequest<PagedListResponse<TenantEmployeeModel>>
{
    /// <summary>
    /// Gets or sets the employee ID to exclude from the potential managers list.
    /// </summary>
    public EmployeeId ExcludeEmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the page number for pagination.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size for pagination.
    /// </summary>
    public int PageSize { get; set; } = 100; // Larger page size for dropdown selection

    /// <inheritdoc/>
    public override ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            TenantId.ValidateRequired(),
            ExcludeEmployeeId.ValidateRequired(),
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}
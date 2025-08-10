// ===========================================================================
// File: GetEmployeesTimelineQuery.cs
// Path: src/dev/Core/EastSeat.ResourceIdea.Application/Features/Employees/Queries/GetEmployeesTimelineQuery.cs
// Description: Query for getting employees timeline data within a date range.
// ===========================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Queries;

/// <summary>
/// Query for getting employees with their work items within a date range for timeline view.
/// </summary>
public sealed class GetEmployeesTimelineQuery : BaseRequest<List<EmployeeTimelineModel>>
{
    /// <summary>
    /// Gets the start date for the timeline period.
    /// </summary>
    public DateOnly StartDate { get; init; }

    /// <summary>
    /// Gets the end date for the timeline period.
    /// </summary>
    public DateOnly EndDate { get; init; }

    /// <summary>
    /// Gets the search term for filtering employees.
    /// </summary>
    public string SearchTerm { get; init; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the GetEmployeesTimelineQuery class.
    /// </summary>
    /// <param name="startDate">The start date for the timeline.</param>
    /// <param name="endDate">The end date for the timeline.</param>
    /// <param name="searchTerm">Optional search term for filtering employees.</param>
    public GetEmployeesTimelineQuery(DateOnly startDate, DateOnly endDate, string searchTerm = "")
    {
        StartDate = startDate;
        EndDate = endDate;
        SearchTerm = searchTerm;
    }

    public override ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            TenantId.ValidateRequired(),
            StartDate <= EndDate ? string.Empty : "Start date must be less than or equal to end date.",
            EndDate >= DateOnly.FromDateTime(DateTime.Today.AddYears(-5)) ? string.Empty : "End date cannot be more than 5 years in the past.",
            StartDate <= DateOnly.FromDateTime(DateTime.Today.AddYears(2)) ? string.Empty : "Start date cannot be more than 2 years in the future."
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}
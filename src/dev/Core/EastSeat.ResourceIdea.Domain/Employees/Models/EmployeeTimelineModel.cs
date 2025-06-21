// ===========================================================================
// File: EmployeeTimelineModel.cs
// Path: src/dev/Core/EastSeat.ResourceIdea.Domain/Employees/Models/EmployeeTimelineModel.cs
// Description: Model for employee timeline view containing employee info and their work items.
// ===========================================================================

using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.WorkItems.Models;

namespace EastSeat.ResourceIdea.Domain.Employees.Models;

/// <summary>
/// Represents an employee with their work items for timeline display.
/// </summary>
public class EmployeeTimelineModel
{
    /// <summary>
    /// Gets or sets the employee ID.
    /// </summary>
    public EmployeeId EmployeeId { get; init; }

    /// <summary>
    /// Gets or sets the employee's first name.
    /// </summary>
    public string FirstName { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee's last name.
    /// </summary>
    public string LastName { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee's email.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee's job position title.
    /// </summary>
    public string JobPositionTitle { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee's department name.
    /// </summary>
    public string DepartmentName { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of work items assigned to this employee.
    /// </summary>
    public List<WorkItemModel> WorkItems { get; init; } = [];

    /// <summary>
    /// Gets the full name of the employee.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}".Trim();
}
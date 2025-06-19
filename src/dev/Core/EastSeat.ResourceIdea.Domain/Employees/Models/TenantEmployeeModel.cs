// ======================================================================
// File: TenantEmployeeModel.cs
// Path: src/dev/Core/EastSeat.ResourceIdea.Domain/Employees/Models/TenantEmployeeModel.cs
// Description: This file contains the definition of the TenantEmployeeModel class.
// ======================================================================

using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Employees.Models;

/// <summary>
/// Tenant employee model.
/// </summary>
public class TenantEmployeeModel
{
    public EmployeeId EmployeeId { get; set; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? EmployeeNumber { get; init; }
    public JobPositionId JobPositionId { get; init; }
    public string? JobPositionTitle { get; init; }
    public DepartmentId DepartmentId { get; init; }
    public string? DepartmentName { get; init; }
    public ApplicationUserId ApplicationUserId { get; init; }
    public TenantId TenantId { get; init; }

    /// <summary>
    /// Represents an empty instance of TenantEmployeeModel with default values.
    /// </summary>
    public static TenantEmployeeModel Empty => new()
    {
        EmployeeId = EmployeeId.Empty,
        JobPositionId = JobPositionId.Empty,
        DepartmentId = DepartmentId.Empty,
        Email = string.Empty,
        EmployeeNumber = string.Empty,
        JobPositionTitle = string.Empty,
        DepartmentName = string.Empty,
        FirstName = string.Empty,
        LastName = string.Empty,
        TenantId = TenantId.Empty
    };

    /// <summary>
    /// Checks if the current instance is empty (equivalent to the Empty property).
    /// </summary>
    public bool IsEmpty => true
        && EmployeeId == EmployeeId.Empty
        && JobPositionId == JobPositionId.Empty
        && DepartmentId == DepartmentId.Empty
        && TenantId == TenantId.Empty
        && string.IsNullOrEmpty(EmployeeNumber)
        && string.IsNullOrEmpty(JobPositionTitle)
        && string.IsNullOrEmpty(DepartmentName)
        && string.IsNullOrEmpty(FirstName)
        && string.IsNullOrEmpty(LastName)
        && string.IsNullOrEmpty(Email);
}

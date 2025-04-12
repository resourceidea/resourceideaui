// ======================================================================
// File: TenantEmployeeModel.cs
// Path: src/dev/Core/EastSeat.ResourceIdea.Domain/Employees/Models/TenantEmployeeModel.cs
// Description: This file contains the definition of the TenantEmployeeModel class.
// ======================================================================

using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Employees.Models;

/// <summary>
/// Tenant employee model.
/// </summary>
public class TenantEmployeeModel
{
    public EmployeeId EmployeeId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public JobPositionId JobPositionId { get; set; }
    public string JobPositionTitle { get; set; } = string.Empty;
    public DepartmentId DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
}

using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Departments.Models;

/// <summary>
/// Represents a department model.
/// </summary>
public class DepartmentModel
{
    /// <summary>
    /// Gets or sets the department ID.
    /// </summary>
    public DepartmentId DepartmentId { get; set; }

    /// <summary>
    /// Gets or sets the tenant ID.
    /// </summary>
    public TenantId TenantId { get; set; }

    /// <summary>
    /// Gets or sets the department name.
    /// </summary>
    public required string DepartmentName { get; set; }
}

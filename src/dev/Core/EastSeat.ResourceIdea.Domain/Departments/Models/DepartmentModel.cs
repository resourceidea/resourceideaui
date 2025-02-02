using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Departments.Models;

/// <summary>
/// Department view model.
/// </summary>
public sealed record DepartmentModel
{
    /// <summary>
    /// Department ID.
    /// </summary>
    public DepartmentId DepartmentId { get; set; }

    /// <summary>
    /// Name of the department.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// ID of tenant owning the department information.
    /// </summary>
    public TenantId TenantId { get; set; }
}

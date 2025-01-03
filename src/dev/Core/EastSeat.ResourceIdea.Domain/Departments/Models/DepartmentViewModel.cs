using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Departments.Models;

/// <summary>
/// Department view model.
/// </summary>
public sealed record DepartmentViewModel
{
    /// <summary>
    /// Department ID.
    /// </summary>
    public DepartmentId DepartmentId { get; set; }

    /// <summary>
    /// Name of the department.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// ID of tenant owning the department information.
    /// </summary>
    public Guid TenantId { get; set; }
}

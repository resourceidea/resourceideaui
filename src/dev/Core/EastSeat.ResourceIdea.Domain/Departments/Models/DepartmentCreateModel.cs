namespace EastSeat.ResourceIdea.Domain.Departments.Models;

/// <summary>
/// Base department model.
/// </summary>
public record DepartmentCreateModel
{
    /// <summary>
    /// Name of the department.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// ID of tenant owning the department information.
    /// </summary>
    public Guid TenantId { get; set; }
}
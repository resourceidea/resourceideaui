using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Departments.Models;

/// <summary>
/// Model representing the department update data.
/// </summary>
public sealed record DepartmentUpdateModel : BaseDepartmentModel
{
    /// <summary>
    /// Department ID.
    /// </summary>
    public DepartmentId DepartmentId { get; set; }
}

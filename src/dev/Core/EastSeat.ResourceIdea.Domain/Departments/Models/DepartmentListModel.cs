using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Departments.Models;

/// <summary>
/// Model representing department listing data.
/// </summary>
public sealed record DepartmentListModel : BaseDepartmentModel
{
    /// <summary>
    /// Department ID.
    /// </summary>
    public DepartmentId DepartmentId { get; init; }

    /// <summary>
    /// Flag indicating whether the department has been deleted or not.
    /// </summary>
    public bool IsDeleted { get; init; }
}

using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Departments.Entities;

public class Department : BaseEntity
{
    /// <summary>
    /// Department ID.
    /// </summary>
    public DepartmentId Id { get; set; }

    /// <summary>
    /// Department name.
    /// </summary>
    public required string Name { get; set; }
}

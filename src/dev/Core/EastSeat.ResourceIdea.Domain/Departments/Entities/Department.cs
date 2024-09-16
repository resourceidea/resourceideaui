using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Departments.Entities;

/// <summary>
/// Department entity class.
/// </summary>
public class Department : BaseEntity
{
    /// <summary>Department Id.</summary>
    public DepartmentId DepartmentId { get; set; }

    /// <summary> Department name. </summary>
    public string DepartmentName { get; set; } = string.Empty;
}

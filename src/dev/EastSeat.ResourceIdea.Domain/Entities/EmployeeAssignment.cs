namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Employee assignment.
/// </summary>
public class EmployeeAssignment : Assignment
{
    /// <summary>
    /// Employee that is assigned to the engagement.
    /// </summary>
    public Guid? EmployeeId { get; set; }

    /// <summary>Employee on assignment.</summary>
    public Employee? Employee { get; set; }
}

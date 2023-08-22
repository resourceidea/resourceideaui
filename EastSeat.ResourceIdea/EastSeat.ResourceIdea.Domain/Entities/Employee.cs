using EastSeat.ResourceIdea.Domain.Common;

namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Employee.
/// </summary>
public class Employee : BaseSubscriptionEntity
{
    /// <summary>Employee Id.</summary>
    public Guid Id { get; set; } = Guid.Empty;

    /// <summary>Job position ID.</summary>
    public Guid? JobPositionId { get; set; }

    /// <summary>Subscription to which the employee belongs to.</summary>
    public Subscription? Subscription { get; set; }

    /// <summary>Job position of the employee.</summary>
    public JobPosition? JobPosition { get; set; }

    /// <summary>Assignments of the employee.</summary>
    public IEnumerable<EmployeeAssignment>? Assignments { get; set; }
}

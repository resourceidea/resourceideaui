using EastSeat.ResourceIdea.Domain.Common;
using EastSeat.ResourceIdea.Domain.Enums;

namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Allocates an employee to an engagement for a period
/// </summary>
public class Assignment : AuditableEntity
{
    private Assignment() { } // EF Core

    public Guid EngagementYearId { get; private set; }
    public Guid EmployeeId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public AllocationType AllocationType { get; private set; }
    public int PercentAllocation { get; private set; }
    public decimal? PlannedHours { get; private set; }
    public string? ColorHint { get; private set; }

    // Navigation properties
    public EngagementYear EngagementYear { get; private set; } = null!;
    public Employee Employee { get; private set; } = null!;

    public static Result<Assignment> Create(Guid engagementYearId, Guid employeeId,
        DateTime startDate, DateTime endDate, AllocationType allocationType,
        int percentAllocation, decimal? plannedHours = null)
    {
        try
        {
            Guard.AgainstInvalidDateRange(startDate, endDate, nameof(startDate));
            Guard.AgainstOutOfRange(percentAllocation, 1, 100, nameof(percentAllocation));

            var assignment = new Assignment
            {
                EngagementYearId = engagementYearId,
                EmployeeId = employeeId,
                StartDate = startDate.Date,
                EndDate = endDate.Date,
                AllocationType = allocationType,
                PercentAllocation = percentAllocation,
                PlannedHours = plannedHours
            };

            return Result<Assignment>.Success(assignment);
        }
        catch (Exception ex)
        {
            return Result<Assignment>.Failure(ex.Message);
        }
    }

    public void UpdateDates(DateTime startDate, DateTime endDate)
    {
        Guard.AgainstInvalidDateRange(startDate, endDate, nameof(startDate));
        StartDate = startDate.Date;
        EndDate = endDate.Date;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateAllocation(int percentAllocation)
    {
        Guard.AgainstOutOfRange(percentAllocation, 1, 100, nameof(percentAllocation));
        PercentAllocation = percentAllocation;
        ModifiedAt = DateTime.UtcNow;
    }
}

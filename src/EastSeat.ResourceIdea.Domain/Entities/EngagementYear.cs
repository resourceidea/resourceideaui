using EastSeat.ResourceIdea.Domain.Common;

namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Annual slice of an engagement for resource planning
/// </summary>
public class EngagementYear : AuditableEntity
{
    private EngagementYear() { } // EF Core

    public Guid EngagementId { get; private set; }
    public int Year { get; private set; }
    public DateTime StartOfYear { get; private set; }
    public DateTime EndOfYear { get; private set; }
    public bool IsLocked { get; private set; }
    public decimal? CarryForwardHours { get; private set; }

    // Navigation properties
    public Engagement Engagement { get; private set; } = null!;
    public ICollection<Assignment> Assignments { get; private set; } = new List<Assignment>();

    public static Result<EngagementYear> Create(Guid engagementId, int year)
    {
        try
        {
            Guard.AgainstOutOfRange(year, 2020, 2100, nameof(year));

            var engagementYear = new EngagementYear
            {
                EngagementId = engagementId,
                Year = year,
                StartOfYear = new DateTime(year, 1, 1),
                EndOfYear = new DateTime(year, 12, 31),
                IsLocked = false
            };

            return Result<EngagementYear>.Success(engagementYear);
        }
        catch (Exception ex)
        {
            return Result<EngagementYear>.Failure(ex.Message);
        }
    }

    public void Lock()
    {
        IsLocked = true;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Unlock()
    {
        IsLocked = false;
        ModifiedAt = DateTime.UtcNow;
    }
}

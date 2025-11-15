using EastSeat.ResourceIdea.Domain.Common;

namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Audit log for engagement year rollover operations
/// </summary>
public class RolloverHistory : AuditableEntity
{
    private RolloverHistory() { } // EF Core

    public Guid EngagementId { get; private set; }
    public int FromYear { get; private set; }
    public int ToYear { get; private set; }
    public DateTime ExecutedAt { get; private set; }
    public Guid ExecutedBy { get; private set; }
    public int AssignmentsCopiedCount { get; private set; }
    public int AssignmentsSkippedCount { get; private set; }
    public string? Notes { get; private set; }

    // Navigation properties
    public Engagement Engagement { get; private set; } = null!;

    public static Result<RolloverHistory> Create(Guid engagementId, int fromYear, int toYear,
        Guid executedBy, int copiedCount, int skippedCount, string? notes = null)
    {
        try
        {
            var history = new RolloverHistory
            {
                EngagementId = engagementId,
                FromYear = fromYear,
                ToYear = toYear,
                ExecutedAt = DateTime.UtcNow,
                ExecutedBy = executedBy,
                AssignmentsCopiedCount = copiedCount,
                AssignmentsSkippedCount = skippedCount,
                Notes = notes?.Trim()
            };

            return Result<RolloverHistory>.Success(history);
        }
        catch (Exception ex)
        {
            return Result<RolloverHistory>.Failure(ex.Message);
        }
    }
}

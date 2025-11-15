using EastSeat.ResourceIdea.Domain.Common;
using EastSeat.ResourceIdea.Domain.Enums;

namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Represents a service engagement with a client
/// </summary>
public class Engagement : AuditableEntity
{
    private Engagement() { } // EF Core

    public Guid ClientId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public EngagementType Type { get; private set; }
    public EngagementStatus Status { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public decimal? BudgetHours { get; private set; }
    public string? Notes { get; private set; }

    // Partners and managers owning the engagement
    public Guid? PartnerId { get; private set; }
    public Guid? ManagerId { get; private set; }

    // Navigation properties
    public Client Client { get; private set; } = null!;
    public Employee? Partner { get; private set; }
    public Employee? Manager { get; private set; }
    public ICollection<EngagementYear> EngagementYears { get; private set; } = new List<EngagementYear>();

    public static Result<Engagement> Create(Guid clientId, string code, string title,
        EngagementType type, DateTime startDate, DateTime? endDate = null,
        Guid? partnerId = null, Guid? managerId = null, decimal? budgetHours = null)
    {
        try
        {
            Guard.AgainstNullOrEmpty(code, nameof(code));
            Guard.AgainstNullOrEmpty(title, nameof(title));

            if (endDate.HasValue)
            {
                Guard.AgainstInvalidDateRange(startDate, endDate.Value, nameof(startDate));
            }

            var engagement = new Engagement
            {
                ClientId = clientId,
                Code = code.Trim().ToUpperInvariant(),
                Title = title.Trim(),
                Type = type,
                Status = EngagementStatus.Planned,
                StartDate = startDate.Date,
                EndDate = endDate?.Date,
                PartnerId = partnerId,
                ManagerId = managerId,
                BudgetHours = budgetHours
            };

            return Result<Engagement>.Success(engagement);
        }
        catch (Exception ex)
        {
            return Result<Engagement>.Failure(ex.Message);
        }
    }

    public void AssignPartner(Guid partnerId)
    {
        PartnerId = partnerId;
        ModifiedAt = DateTime.UtcNow;
    }

    public void AssignManager(Guid managerId)
    {
        ManagerId = managerId;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(EngagementStatus status)
    {
        Status = status;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        Status = EngagementStatus.Completed;
        EndDate ??= DateTime.UtcNow.Date;
        ModifiedAt = DateTime.UtcNow;
    }
}

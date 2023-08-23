using EastSeat.ResourceIdea.Domain.Common;

namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Subscribing entity job positions.
public class JobPosition : BaseSubscriptionEntity
{
    /// <summary>Job position ID.</summary>
    public Guid Id { get; set; } = Guid.Empty;

    /// <summary>Job position title.</summary>
    public string? Title { get; set; }

    /// <summary>Job position description.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Subscription to which the job position belongs to.</summary>
    public Subscription? Subscription { get; set; }

    /// <summary>Employees on the job position.</summary>
    public IEnumerable<Employee>? Employees { get; set; }
}

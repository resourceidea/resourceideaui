using EastSeat.ResourceIdea.Domain.Common;

namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Subscriber's engagements with the different clients.
/// </summary>
public class Engagement : BaseSubscriptionEntity
{
    /// <summary>Engagement ID.</summary>
    public Guid Id { get; set; } = Guid.Empty;

    /// <summary>Engagement's client ID.</summary>
    public Guid? ClientId { get; set; }

    /// <summary>Date when the engagement was started.</summary>
    public DateTime? StartDate { get; set; }

    /// <summary>Date when the engagement was closed.</summary>
    public DateTime? EndDate { get; set; }

    /// <summary>Subscription to which the engagement belongs to.</summary>
    public Subscription? Subscription { get; set; }

    /// <summary>Engagement's client.</summary>
    public Client? Client { get; set; }

    /// <summary>Assignments on the engagement.</summary>
    public IEnumerable<Assignment>? Assignments { get; set; }
}

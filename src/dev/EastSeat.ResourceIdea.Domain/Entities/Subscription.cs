using EastSeat.ResourceIdea.Domain.Common;
using EastSeat.ResourceIdea.Domain.Constants;

namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Service subscription details.
/// </summary>
public class Subscription : BaseSubscriptionEntity
{
    /// <summary>Name of the subscribing entity.</summary>
    public string? SubscriberName { get; set; }

    /// <summary>Date the subscription was started.</summary>
    public DateTime StartDate { get; set; }

    /// <summary>Date the subscription was terminated.</summary>
    public DateTime? TerminationDate { get; set; }

    /// <summary>Date the subscription was renewed.</summary>
    public DateTime? LastRenewalDate { get; set; }

    /// <summary>Subscription status.</summary>
    public SubscriptionStatus Status { get; set; }

    /// <summary>Clients under the subscription.</summary>
    public IEnumerable<Client>? Clients { get; set; }

    /// <summary>Engagements under the subscription.</summary>
    public IEnumerable<Engagement>? Engagements { get; set; }

    /// <summary>Assets under the subscription.</summary>
    public IEnumerable<Asset>? Assets { get; set; }

    /// <summary>Assignments under the subscription.</summary>
    public IEnumerable<Assignment>? Assignments { get; set; }

    /// <summary>Employees under the subscription.</summary>
    public IEnumerable<Employee>? Employees { get; set; }

    /// <summary>Job positions under the subscription.</summary>
    public IEnumerable<JobPosition>? JobPositions { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EastSeat.ResourceIdea.Domain.Entities;
using EastSeat.ResourceIdea.Domain.Enums;

namespace EastSeat.ResourceIdea.Persistence.Models;

/// <summary>
/// Subscription entity.
/// </summary>
public class SubscriptionEntity : BaseEntity
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
    public IEnumerable<ClientEntity>? Clients { get; set; }
}

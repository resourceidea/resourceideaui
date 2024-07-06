using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Engagements.Models;

/// <summary>
/// Represents an engagement model.
/// </summary>
public record EngagementModel
{
    /// <summary>
    /// Gets or sets the engagement ID.
    /// </summary>
    public EngagementId Id { get; init; }

    /// <summary>
    /// Gets or sets the client ID.
    /// </summary>
    public ClientId ClientId { get; init; }

    /// <summary>
    /// Gets or sets the tenant ID.
    /// </summary>
    public TenantId TenantId { get; set; }

    /// <summary>
    /// Gets or sets the commencement date of the engagement.
    /// </summary>
    public DateTimeOffset CommencementDate { get; init; }

    /// <summary>
    /// Gets or sets the completion date of the engagement.
    /// </summary>
    public DateTimeOffset CompletionDate { get; init; }

    /// <summary>
    /// Gets or sets the status of the engagement.
    /// </summary>
    public EngagementStatus Status { get; init; }
}

using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Engagements.Models;

/// <summary>
/// Represents an engagement model.
/// </summary>
public record EngagementModel
{    /// <summary>
     /// Gets or sets the engagement ID.
     /// </summary>
    public EngagementId Id { get; init; }

    /// <summary>
    /// Gets or sets the title of the engagement.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the engagement.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the client ID.
    /// </summary>
    public ClientId ClientId { get; init; }

    /// <summary>
    /// Name of the client associated with the engagement.
    /// </summary>
    public string ClientName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the tenant ID.
    /// </summary>
    public TenantId TenantId { get; set; }

    /// <summary>
    /// Gets or sets the start date of the engagement.
    /// </summary>
    public DateTimeOffset? StartDate { get; init; }

    /// <summary>
    /// Gets or sets the end date of the engagement.
    /// </summary>
    public DateTimeOffset? EndDate { get; init; }    /// <summary>
                                                     /// Gets or sets the status of the engagement.
                                                     /// </summary>
    public EngagementStatus Status { get; init; }

    /// <summary>
    /// Gets or sets the manager ID responsible for the engagement.
    /// </summary>
    public EmployeeId? ManagerId { get; init; }

    /// <summary>
    /// Gets or sets the partner ID responsible for the engagement.
    /// </summary>
    public EmployeeId? PartnerId { get; init; }
}

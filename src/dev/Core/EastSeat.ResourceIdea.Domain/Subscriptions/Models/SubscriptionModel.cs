using EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Subscriptions.Models;

/// <summary>
/// Subscription model.
/// </summary>
public record SubscriptionModel
{
    /// <summary>
    /// Subscription DepartmentId.
    /// </summary>
    public SubscriptionId Id { get; set; }

    /// <summary>
    /// Tenant DepartmentId.
    /// </summary>
    public TenantId TenantId { get; set; }

    /// <summary>
    /// DepartmentId of the service subscribed to by the tenant.
    /// </summary>
    public SubscriptionServiceId SubscriptionServiceId { get; set; }

    /// <summary>
    /// Name of the service subscribed to by the tenant.
    /// </summary>
    public string SubscriptionServiceName { get; set; } = string.Empty;
}
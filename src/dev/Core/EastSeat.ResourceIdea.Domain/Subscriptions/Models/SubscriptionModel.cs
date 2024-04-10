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
    /// Subscription Id.
    /// </summary>
    public SubscriptionId Id { get; set; }

    /// <summary>
    /// Tenant Id.
    /// </summary>
    public TenantId TenantId { get; set; }

    /// <summary>
    /// Id of the service subscribed to by the tenant.
    /// </summary>
    public SubscriptionServiceId SubscriptionServiceId { get; set; }

    /// <summary>
    /// Name of the service subscribed to by the tenant.
    /// </summary>
    public string SubscriptionServiceName { get; set; } = string.Empty;
}
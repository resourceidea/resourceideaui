using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Enums;
using EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Domain.Subscriptions.Entities;

/// <summary>
/// Subscription entity.
/// </summary>
public class Subscription : BaseEntity
{
    /// <summary>
    /// Subscription unique identifier.
    /// </summary>
    public SubscriptionId Id { get; set; }

    /// <summary>
    /// Date when the subscription was made by the tenant.
    /// </summary>
    public DateTimeOffset? SubscriptionDate { get; set; }

    /// <summary>
    /// Status of the subscription.
    /// </summary>
    public SubscriptionStatus Status { get; set; }

    /// <summary>
    /// Date when the subscription was cancelled by the tenant.
    /// </summary>
    public DateTimeOffset? SubscriptionCancellationDate { get; set; }

    /// <summary>
    /// DepartmentId of the service subscribed to by the tenant.
    /// </summary>
    public SubscriptionServiceId SubscriptionServiceId { get; set; }
    
    /// <summary>
    /// Type of subscription for the service.
    /// </summary>
    public SubscriptionType SubscriptionType { get; set; }

    /// <summary>
    /// Subscription service.
    /// </summary>
    public SubscriptionService? SubscriptionService { get; set; }

    public override TModel ToModel<TModel>()
    {
        throw new NotImplementedException();
    }

    public override ResourceIdeaResponse<TModel> ToResourceIdeaResponse<TEntity, TModel>()
    {
        throw new NotImplementedException();
    }
}
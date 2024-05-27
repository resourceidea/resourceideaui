using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands;

/// <summary>
/// Command to create a subscription.
/// </summary>
public sealed class CreateSubscriptionCommand : IRequest<ResourceIdeaResponse<SubscriptionModel>> {
    /// <summary>
    /// Id of the tenant to create the subscription for.
    /// </summary>
    public TenantId TenantId { get; set; }

    /// <summary>
    /// Id of the subscription service to subscribe to.
    /// </summary>
    public SubscriptionServiceId SubscriptionServiceId { get; set; }
 }
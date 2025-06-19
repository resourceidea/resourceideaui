using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands;

/// <summary>
/// Command to create a subscription.
/// </summary>
public sealed class CreateSubscriptionCommand : IRequest<ResourceIdeaResponse<SubscriptionModel>>
{
    /// <summary>
    /// DepartmentId of the tenant to create the subscription for.
    /// </summary>
    public TenantId TenantId { get; set; }

    /// <summary>
    /// DepartmentId of the subscription service to subscribe to.
    /// </summary>
    public SubscriptionServiceId SubscriptionServiceId { get; set; }
}
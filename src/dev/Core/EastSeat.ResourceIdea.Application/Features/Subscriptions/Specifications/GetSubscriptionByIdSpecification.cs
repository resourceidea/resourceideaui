using System.Linq.Expressions;

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Specifications;

/// <summary>
/// Specification to get subscription by SubscriptionId.
/// </summary>
/// <param name="subscriptionId">Subscription SubscriptionId.</param>
public sealed class GetSubscriptionByIdSpecification(SubscriptionId subscriptionId) : BaseSpecification<Subscription>
{
    private readonly SubscriptionId _subscriptionId = subscriptionId;

    public override Expression<Func<Subscription, bool>> Criteria => subscription => subscription.Id == _subscriptionId;
}

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.SubscriptionServiceManagement.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServiceManagement.ValueObjects;
using EastSeat.ResourceIdea.Domain.TenantManagement.Entities;
using EastSeat.ResourceIdea.Domain.TenantManagement.ValueObjects;
using System.Linq.Expressions;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Specifications;

/// <summary>
/// Specification to get a subscription service by Id.
/// </summary>
/// <param name="subscriptionServiceId">Subscription service Id.</param>
public sealed class SubscriptionServiceGetByIdSpecification(SubscriptionServiceId subscriptionServiceId) : BaseSpecification<SubscriptionService>
{
    private readonly SubscriptionServiceId _subscriptionServiceId = subscriptionServiceId;

    public override Expression<Func<SubscriptionService, bool>> Criteria => subscriptionService => subscriptionService.Id == _subscriptionServiceId;
}
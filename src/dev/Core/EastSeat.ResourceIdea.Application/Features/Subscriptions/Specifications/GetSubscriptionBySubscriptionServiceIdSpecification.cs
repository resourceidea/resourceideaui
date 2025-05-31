﻿using System.Linq.Expressions;

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Specifications;

/// <summary>
/// Specification used to filter subscriptions that belong to a specific subscription service.
/// </summary>
/// <param name="subscriptionServiceId">Subscription service DepartmentId.</param>
public sealed class GetSubscriptionBySubscriptionServiceIdSpecification(
    Dictionary<string, string>? filters) : BaseSpecification<Subscription>
{
    private readonly Dictionary<string, string>? _filters = filters;

    public override Expression<Func<Subscription, bool>> Criteria
    {
        get
        {
            Guid? filterValidationResult = GetValidatedSubscriptionServiceId();
            Guid filter = filterValidationResult ?? Guid.Empty;

            return Guid.Empty == filter
                ? subscription => false
                : subscription => subscription.SubscriptionServiceId.Value == filter;
        }
    }

    private Guid? GetValidatedSubscriptionServiceId()
    {
        if (_filters is null
            || _filters.Count <= 0
            || !_filters.TryGetValue("subscriptionServiceId", out var subscriptionServiceIdValue)
            || string.IsNullOrEmpty(subscriptionServiceIdValue))
        {
            return null;
        }

        if (!Guid.TryParse(subscriptionServiceIdValue, out Guid subscriptionServiceIdGuid))
        {
            return null;
        }

        return subscriptionServiceIdGuid;
    }
}

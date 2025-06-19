﻿using System.Linq.Expressions;

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Specifications;

/// <summary>
/// Specification to query subscriptions by subscription date.
/// </summary>
public sealed class GetSubscriptionBySubscribedBeforeDateSpecification(Dictionary<string, string>? filters)
    : BaseSpecification<Subscription>
{
    private readonly Dictionary<string, string>? _filters = filters;

    public override Expression<Func<Subscription, bool>> Criteria
    {
        get
        {
            Optional<DateTimeOffset> filterValidationResult = GetValidatedDateTimeOffset();
            DateTimeOffset filter = filterValidationResult.Match(
                some: value => value,
                none: () => DateTimeOffset.MinValue);

            return filter == DateTimeOffset.MinValue
                ? subscription => false
                : subscription => subscription.SubscriptionDate <= filter;
        }
    }

    private Optional<DateTimeOffset> GetValidatedDateTimeOffset()
    {
        if (_filters is null
            || _filters.Count <= 0
            || !_filters.TryGetValue("subbefore", out var filterDateValue)
            || string.IsNullOrEmpty(filterDateValue))
        {
            return Optional<DateTimeOffset>.None;
        }

        if (!DateTimeOffset.TryParse(filterDateValue, out DateTimeOffset filterDate))
        {
            return Optional<DateTimeOffset>.None;
        }

        return Optional<DateTimeOffset>.Some(filterDate);
    }
}

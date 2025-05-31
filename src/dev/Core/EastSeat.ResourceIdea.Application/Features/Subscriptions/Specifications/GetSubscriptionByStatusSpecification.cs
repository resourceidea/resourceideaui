using System.Linq.Expressions;

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Specifications;

public sealed class GetSubscriptionByStatusSpecification (Dictionary<string, string>? filters) : BaseSpecification<Subscription>
{
    private readonly Dictionary<string, string>? _filters = filters;

    public override Expression<Func<Subscription, bool>> Criteria
    {
        get
        {
            string? filterValidationResult = GetValidatedStatusFilter();
            string filter = filterValidationResult ?? string.Empty;

            return string.IsNullOrEmpty(filter)
                ? subscription => false
                : subscription => subscription.Status.ToString() == filter;
        }
    }

    private string? GetValidatedStatusFilter()
    {
        if (_filters is null
            || _filters.Count <= 0
            || !_filters.TryGetValue("status", out var statusValue)
            || string.IsNullOrEmpty(statusValue))
        {
            return null;
        }

        return statusValue;
    }
}
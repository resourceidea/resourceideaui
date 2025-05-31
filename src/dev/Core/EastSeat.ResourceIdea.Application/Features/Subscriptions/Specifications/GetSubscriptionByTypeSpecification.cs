using System.Linq.Expressions;

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Specifications;

/// <summary>
/// Specification to filter subscriptions by type.
/// </summary>
public sealed class GetSubscriptionByTypeSpecification(Dictionary<string, string>? filters):
    BaseSpecification<Subscription>
{
    private readonly Dictionary<string, string>? _filters = filters;

    public override Expression<Func<Subscription, bool>> Criteria
    {
        get
        {
            string? filterValidationResult = GetValidatedTypeFilter();
            string filter = filterValidationResult ?? string.Empty;

            return string.IsNullOrEmpty(filter)
                ? subscription => false
                : subscription => subscription.SubscriptionType.ToString() == filter;
        }
    }

    private string? GetValidatedTypeFilter()
    {
        if (_filters is null
            || _filters.Count <= 0
            || !_filters.TryGetValue("type", out var typeValue)
            || string.IsNullOrEmpty(typeValue))
        {
            return null;
        } 

        return typeValue;
    }
}

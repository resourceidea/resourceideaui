using System.Linq.Expressions;

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;

using Optional;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Specifications;

/// <summary>
/// Specification used to filter subscription services whose name contains the filter provided.
/// </summary>
/// <param name="filters">Values used by the specification to filter the values returned 
/// when querying for subscription services.</param>
public sealed class SubscriptionServiceNameSpecification (Dictionary<string, string>? filters) : BaseSpecification<SubscriptionService>
{
    private readonly Dictionary<string, string>? _filters = filters;

    public override Expression<Func<SubscriptionService, bool>> Criteria
    {
        get
        {
            Option<string> filterValidationResult = GetValidatedNameFilter();
            string filter = filterValidationResult.Match(
                some: value => value,
                none: () => string.Empty);

            return string.IsNullOrEmpty(filter)
                ? subscriptionService => false
                : subscriptionService => subscriptionService.Name.Contains(filter);
        }
    }

    private Option<string> GetValidatedNameFilter()
    {
        if (_filters is null
            || _filters.Count <= 0
            || !_filters.TryGetValue("name", out var nameValue)
            || string.IsNullOrEmpty(nameValue))
        {
            return Option.None<string>();
        }

        return Option.Some<string>(nameValue);
    }
}

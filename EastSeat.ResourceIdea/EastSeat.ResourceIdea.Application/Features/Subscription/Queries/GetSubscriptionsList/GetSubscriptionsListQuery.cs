using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscription.Queries.GetSubscriptionsList
{
    /// <summary>
    /// Query to get a list of subscriptions.
    /// </summary>
    public class GetSubscriptionsListQuery : IRequest<IEnumerable<SubscriptionsListVM>>
    {
    }
}

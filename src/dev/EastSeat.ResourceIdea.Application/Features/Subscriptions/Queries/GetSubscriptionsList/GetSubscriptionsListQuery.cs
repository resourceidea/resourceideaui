using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Queries.GetSubscriptionsList
{
    /// <summary>
    /// Query to get a list of subscriptions.
    /// </summary>
    public class GetSubscriptionsListQuery : IRequest<IEnumerable<SubscriptionsListVM>>
    {
    }
}

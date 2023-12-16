using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscription.Queries.GetSubscriptionsList;

/// <summary>
/// Handles the query to get a list of subscriptions.
/// </summary>
/// <remarks>
/// Instantiates <see cref="GetSubscriptionsListQueryHandler"/>.
/// </remarks>
/// <param name="mapper">Mapper.</param>
/// <param name="subscriptionRepository">Subscription repository.</param>
public class GetSubscriptionsListQueryHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Subscription> subscriptionRepository) : IRequestHandler<GetSubscriptionsListQuery, IEnumerable<SubscriptionsListVM>>
{
    private readonly IMapper mapper = mapper;
    private readonly IAsyncRepository<Domain.Entities.Subscription> subscriptionRepository = subscriptionRepository;

    /// <inheritdoc />
    public async Task<IEnumerable<SubscriptionsListVM>> Handle(GetSubscriptionsListQuery request, CancellationToken cancellationToken)
    {
        var subscriptions = await subscriptionRepository.ListAllAsync();

        return mapper.Map<IEnumerable<SubscriptionsListVM>>(subscriptions);
    }
}

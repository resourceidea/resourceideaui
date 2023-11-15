using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscription.Queries.GetSubscriptionsList;

/// <summary>
/// Handles the query to get a list of subscriptions.
/// </summary>
public class GetSubscriptionsListQueryHandler : IRequestHandler<GetSubscriptionsListQuery, IEnumerable<SubscriptionsListVM>>
{
    private readonly IMapper mapper;
    private readonly IAsyncRepository<Domain.Entities.Subscription> subscriptionRepository;

    /// <summary>
    /// Instantiates <see cref="GetSubscriptionsListQueryHandler"/>.
    /// </summary>
    /// <param name="mapper">Mapper.</param>
    /// <param name="subscriptionRepository">Subscription repository.</param>
    public GetSubscriptionsListQueryHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Subscription> subscriptionRepository)
    {
        this.mapper = mapper;
        this.subscriptionRepository = subscriptionRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SubscriptionsListVM>> Handle(GetSubscriptionsListQuery request, CancellationToken cancellationToken)
    {
        var subscriptions = await subscriptionRepository.ListAllAsync();

        return mapper.Map<IEnumerable<SubscriptionsListVM>>(subscriptions);
    }
}

using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Queries;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;

using MediatR;

using Optional;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Handlers;

public sealed class GetSubscriptionByIdQueryHandler(
    IAsyncRepository<Subscription> subscriptionRepository,
    IMapper mapper) : IRequestHandler<GetSubscriptionByIdQuery, ResourceIdeaResponse<SubscriptionModel>>
{
    private readonly IAsyncRepository<Subscription> _subscriptionRepository = subscriptionRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<SubscriptionModel>> Handle(GetSubscriptionByIdQuery request, CancellationToken cancellationToken)
    {
        GetSubscriptionByIdSpecification getSubscriptionByIdSpecification = new (request.SubscriptionId);
        Option<Subscription> getSubscriptionResult = await _subscriptionRepository.GetByIdAsync(getSubscriptionByIdSpecification, cancellationToken);
        Subscription subscription = getSubscriptionResult.Match(
            some: subscription => subscription,
            none: () => EmptySubscription.Instance);

        if (subscription == EmptySubscription.Instance)
        {
            return ResourceIdeaResponse<SubscriptionModel>.NotFound();
        }

        return new ResourceIdeaResponse<SubscriptionModel>
        {
            Success = true,
            Content = Option.Some(_mapper.Map<SubscriptionModel>(subscription))
        };
    }
}

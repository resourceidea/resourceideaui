using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands;
using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Enums;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

using MediatR;

using Optional;

namespace EastSeat.ResourceIdea.Application.Features.Subscriptions.Handlers;

public sealed class CreateSubscriptionCommandHandler(
    IAsyncRepository<Subscription> subscriptionRepository,
    IMapper mapper)
    : IRequestHandler<CreateSubscriptionCommand, ResourceIdeaResponse<SubscriptionModel>>
{
    private readonly IAsyncRepository<Subscription> _subscriptionRepository = subscriptionRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<SubscriptionModel>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var subscription = new Subscription()
        {
            Id = SubscriptionId.Create(Guid.NewGuid()),
            Status = SubscriptionStatus.Active,
            SubscriptionDate = DateTimeOffset.UtcNow,
            SubscriptionType = SubscriptionType.Monthly,
            SubscriptionServiceId = SubscriptionServiceId.Create(Guid.NewGuid()), // TODO: Get the susbcription service id from the user who made request.
            TenantId = TenantId.Create(Guid.NewGuid()).Value // TODO: Get the tenant id from the user who made request.
        };
        var addedSubscription = await _subscriptionRepository.AddAsync(subscription, cancellationToken);

        return new ResourceIdeaResponse<SubscriptionModel>
        {
            Success = true,
            Content = Option.Some(_mapper.Map<SubscriptionModel>(addedSubscription))
        };
    }
}

using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Subscriptions.Commands;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Subscriptions.Entities;
using EastSeat.ResourceIdea.Domain.Subscriptions.Enums;
using EastSeat.ResourceIdea.Domain.Subscriptions.Models;
using EastSeat.ResourceIdea.Domain.Subscriptions.ValueObjects;

using MediatR;

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
            SubscriptionServiceId = request.SubscriptionServiceId,
            TenantId = request.TenantId.Value
        };
        
        var addedSubscription = await _subscriptionRepository.AddAsync(subscription, cancellationToken);

        return new ResourceIdeaResponse<SubscriptionModel>
        {
            Success = true,
            Content = Optional<SubscriptionModel>.Some(_mapper.Map<SubscriptionModel>(addedSubscription))
        };
    }
}

using AutoMapper;
using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Asset.Commands;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Subscription.Commands.CreateSubscription;

/// <summary>
/// Handles the create subscription command.
/// </summary>
public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, CreateSubscriptionCommandResponse>
{
    private readonly IMapper mapper;
    private readonly IAsyncRepository<Domain.Entities.Subscription> subscriptionRepository;

    /// <summary>
    /// Initializes <see cref="CreateSubscriptionCommandHandler"/>.
    /// </summary>
    /// <param name="mapper">Mapper.</param>
    /// <param name="subscriptionRepository">Subscription repository.</param>
    public CreateSubscriptionCommandHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Subscription> subscriptionRepository)
    {
        this.mapper = mapper;
        this.subscriptionRepository = subscriptionRepository;
    }

    /// <inheritdoc />
    public async Task<CreateSubscriptionCommandResponse> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var createSubscriptionCommandResponse = new CreateSubscriptionCommandResponse();

        var validator = new CreateSubscriptionCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count > 0)
        {
            createSubscriptionCommandResponse.Success = false;
            createSubscriptionCommandResponse.ValidationErrors = new List<string>();
            foreach (var error in validationResult.Errors)
            {
                createSubscriptionCommandResponse.ValidationErrors.Add(error.ErrorMessage);
            }
        }

        if (createSubscriptionCommandResponse.Success)
        {
            var subscription = new Domain.Entities.Subscription
            {
                StartDate = request.StartDate,
                SubscriberName = request.SubscriberName,
                SubscriptionId = request.SubscriptionId,
                Status = request.Status
            };
            subscription = await subscriptionRepository.AddAsync(subscription);
            createSubscriptionCommandResponse.Subscription = mapper.Map<CreateSubscriptionVM>(subscription);
        }

        return createSubscriptionCommandResponse;
    }
}

using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Commands;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Validators;
using EastSeat.ResourceIdea.Application.Constants;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;

using MediatR;

using Optional;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Handlers;

public sealed class CreateSubscriptionServiceCommandHandler(
    IAsyncRepository<SubscriptionService> subscriptionServiceRepository,
    IMapper mapper)
        : IRequestHandler<CreateSubscriptionServiceCommand, ResourceIdeaResponse<SubscriptionServiceModel>>
{
    private readonly IAsyncRepository<SubscriptionService> _subscriptionServiceRepository = subscriptionServiceRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<SubscriptionServiceModel>> Handle(CreateSubscriptionServiceCommand request, CancellationToken cancellationToken)
    {
        CreateSubscriptionServiceCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid || validationResult.Errors.Count > 0)
        {
            return new ResourceIdeaResponse<SubscriptionServiceModel>
            {
                Success = false,
                Message = "Create subscription service command validation failed",
                ErrorCode = ErrorCodes.CreateSubscriptionServiceCommandValidationFailure.ToString(),
                Content = Option.None<SubscriptionServiceModel>()
            };
        }

        SubscriptionService subscriptionService = new()
        {
            Id = SubscriptionServiceId.Create(Guid.NewGuid()),
            Name = request.Name
        };

        SubscriptionService newSubscriptionService = await _subscriptionServiceRepository.AddAsync(subscriptionService, cancellationToken);

        return new ResourceIdeaResponse<SubscriptionServiceModel>
        {
            Success = true,
            Message = "Subscription service created successfully",
            Content = Option.Some(_mapper.Map<SubscriptionServiceModel>(newSubscriptionService))
        };
    }
}
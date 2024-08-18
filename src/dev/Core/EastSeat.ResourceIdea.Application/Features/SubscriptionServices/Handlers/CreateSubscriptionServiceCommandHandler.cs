using AutoMapper;
using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Commands;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Validators;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Handlers;

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
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            return ResourceIdeaResponse<SubscriptionServiceModel>.Failure(ErrorCode.CreateSubscriptionServiceCommandValidationFailure);
        }

        SubscriptionService subscriptionService = new()
        {
            Id = SubscriptionServiceId.Create(Guid.NewGuid()),
            Name = request.Name
        };

        var newSubscriptionServiceResult = await _subscriptionServiceRepository.AddAsync(subscriptionService, cancellationToken);
        if (newSubscriptionServiceResult.IsFailure)
        {
            return ResourceIdeaResponse<SubscriptionServiceModel>.Failure(newSubscriptionServiceResult.Error);
        }

        return ResourceIdeaResponse<SubscriptionServiceModel>
                    .Success(Optional<SubscriptionServiceModel>.Some(_mapper.Map<SubscriptionServiceModel>(newSubscriptionServiceResult)));
    }
}
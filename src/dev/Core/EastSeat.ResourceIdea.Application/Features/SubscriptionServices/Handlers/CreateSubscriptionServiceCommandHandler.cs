using AutoMapper;
using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Commands;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Validators;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Handlers;

public sealed class CreateSubscriptionServiceCommandHandler(
    ISubscriptionServicesService subscriptionServicesService,
    IMapper mapper)
        : IRequestHandler<CreateSubscriptionServiceCommand, ResourceIdeaResponse<SubscriptionServiceModel>>
{
    private readonly ISubscriptionServicesService _subscriptionServicesService = subscriptionServicesService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<SubscriptionServiceModel>> Handle(CreateSubscriptionServiceCommand request, CancellationToken cancellationToken)
    {
        CreateSubscriptionServiceCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            return ResourceIdeaResponse<SubscriptionServiceModel>.Failure(ErrorCode.CommandValidationFailure);
        }

        // TODO: Map CreateSubscriptionServiceCommand class to the SubscriptionService class.
        var subscriptionService = _mapper.Map<SubscriptionService>(request);
        var response = await _subscriptionServicesService.AddAsync(subscriptionService, cancellationToken);

        return _mapper.Map<ResourceIdeaResponse<SubscriptionServiceModel>>(response);
    }
}
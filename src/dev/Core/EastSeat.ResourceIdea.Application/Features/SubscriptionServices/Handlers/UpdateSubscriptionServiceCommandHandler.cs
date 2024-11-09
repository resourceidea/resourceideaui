using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Commands;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Validators;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;

using MediatR;
using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Contracts;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Handlers;

public sealed class UpdateSubscriptionServiceCommandHandler(
    ISubscriptionServicesService subscriptionServicesService,
    IMapper mapper) : IRequestHandler<UpdateSubscriptionServiceCommand, ResourceIdeaResponse<SubscriptionServiceModel>>
{
    private readonly ISubscriptionServicesService _subscriptionServicesService = subscriptionServicesService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<SubscriptionServiceModel>> Handle(
        UpdateSubscriptionServiceCommand request,
        CancellationToken cancellationToken)
    {
        UpdateSubscriptionServiceCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            return ResourceIdeaResponse<SubscriptionServiceModel>.Failure(ErrorCode.CommandValidationFailure);
        }

        // TODO: Map UpdateSubscriptionServiceCommand to SubscriptionService class.
        var subscriptionService = _mapper.Map<SubscriptionService>(request);
        var response = await _subscriptionServicesService.UpdateAsync(subscriptionService, cancellationToken);

        return _mapper.Map<ResourceIdeaResponse<SubscriptionServiceModel>>(response);
    }
}

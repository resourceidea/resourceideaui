using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Commands;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Validators;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;

using MediatR;
using EastSeat.ResourceIdea.Application.Enums;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServiceManagement.Handlers;

public sealed class UpdateSubscriptionServiceCommandHandler(
    IAsyncRepository<SubscriptionService> subscriptionServiceRepository,
    IMapper mapper) : IRequestHandler<UpdateSubscriptionServiceCommand, ResourceIdeaResponse<SubscriptionServiceModel>>
{
    private readonly IAsyncRepository<SubscriptionService> _subscriptionServiceRepository = subscriptionServiceRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<SubscriptionServiceModel>> Handle(
        UpdateSubscriptionServiceCommand request,
        CancellationToken cancellationToken)
    {
        UpdateSubscriptionServiceCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            return ResourceIdeaResponse<SubscriptionServiceModel>.Failure(ErrorCode.UpdateSubscriptionServiceCommandValidationFailure);
        }

        SubscriptionService subscriptionService = new()
        {
            Id = request.Id,
            Name = request.Name
        };
        var subscriptionServiceUpdateResult = await _subscriptionServiceRepository.UpdateAsync(subscriptionService, cancellationToken);
        if (subscriptionServiceUpdateResult.IsFailure)
        {
            return ResourceIdeaResponse<SubscriptionServiceModel>.Failure(subscriptionServiceUpdateResult.Error);
        }

        return ResourceIdeaResponse<SubscriptionServiceModel>.Success(Optional<SubscriptionServiceModel>
                    .Some(_mapper.Map<SubscriptionServiceModel>(subscriptionServiceUpdateResult.Content.Value)));
    }
}

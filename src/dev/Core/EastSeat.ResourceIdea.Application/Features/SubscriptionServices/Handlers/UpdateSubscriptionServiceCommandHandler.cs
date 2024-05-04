using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Commands;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Validators;
using EastSeat.ResourceIdea.Application.Constants;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;

using MediatR;

using Optional;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;

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
        if (validationResult.IsValid || validationResult.Errors.Count > 0)
        {
            return new ResourceIdeaResponse<SubscriptionServiceModel>
            {
                Success = false,
                Message = "Subscription service update command validation failed",
                ErrorCode = ErrorCodes.UpdateSubscriptionServiceCommandValidationFailure.ToString(),
                Content = Option.None<SubscriptionServiceModel>()
            };
        }

        SubscriptionService subscriptionService = new()
        {
            Id = request.Id,
            Name = request.Name
        };

        Option<SubscriptionService> subscriptionServiceUpdateResult = await _subscriptionServiceRepository.UpdateAsync(
            subscriptionService,
            cancellationToken);
        SubscriptionService updatedSubscriptionService = subscriptionServiceUpdateResult.Match(
            some: subscriptionService => subscriptionService,
            none: () => EmptySubscriptionService.Instance);

        return new ResourceIdeaResponse<SubscriptionServiceModel>
        {
            Success = true,
            Message = "Subscription service updated successfully",
            Content = Option.Some(_mapper.Map<SubscriptionServiceModel>(updatedSubscriptionService))
        };
    }
}

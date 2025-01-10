using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Commands;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Validators;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;

using MediatR;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Contracts;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Application.Features.Common.Handlers;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Handlers;

/// <summary>
/// Handles the command to update a subscription service.
/// </summary>
/// <param name="subscriptionServicesService">The subscription services service.</param>
public sealed class UpdateSubscriptionServiceCommandHandler(ISubscriptionServicesService subscriptionServicesService)
    : BaseHandler, IRequestHandler<UpdateSubscriptionServiceCommand, ResourceIdeaResponse<SubscriptionServiceModel>>
{
    private readonly ISubscriptionServicesService _subscriptionServicesService = subscriptionServicesService;

    /// <summary>
    /// Handles the update subscription service command.
    /// </summary>
    /// <param name="command">The command to update a subscription service.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A response containing the updated subscription service model or an error code.</returns>
    public async Task<ResourceIdeaResponse<SubscriptionServiceModel>> Handle(
        UpdateSubscriptionServiceCommand command,
        CancellationToken cancellationToken)
    {
        UpdateSubscriptionServiceCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            return ResourceIdeaResponse<SubscriptionServiceModel>.Failure(ErrorCode.CommandValidationFailure);
        }

        var subscriptionService = command.ToEntity();
        var response = await _subscriptionServicesService.UpdateAsync(subscriptionService, cancellationToken);
        var handlerResponse = GetHandlerResponse<SubscriptionService, SubscriptionServiceModel>(response, ErrorCode.EmptyEntityOnUpdateSubscriptionService);

        return handlerResponse;
    }
}

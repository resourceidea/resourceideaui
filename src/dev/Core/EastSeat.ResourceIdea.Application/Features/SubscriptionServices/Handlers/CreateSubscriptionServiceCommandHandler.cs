using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Commands;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Contracts;
using EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Validators;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.SubscriptionServices.Handlers;


/// <summary>
/// Handles the creation of a subscription service.
/// </summary>
/// <param name="subscriptionServicesService">The subscription services service.</param>
public sealed class CreateSubscriptionServiceCommandHandler(ISubscriptionServicesService subscriptionServicesService)
    : BaseHandler, IRequestHandler<CreateSubscriptionServiceCommand, ResourceIdeaResponse<SubscriptionServiceModel>>
{
    private readonly ISubscriptionServicesService _subscriptionServicesService = subscriptionServicesService;

    /// <summary>
    /// Handles the creation of a subscription service.
    /// </summary>
    /// <param name="command">The command containing the details of the subscription service to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A response containing the created subscription service model or an error code.</returns>
    public async Task<ResourceIdeaResponse<SubscriptionServiceModel>> Handle(CreateSubscriptionServiceCommand command, CancellationToken cancellationToken)
    {
        CreateSubscriptionServiceCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            return ResourceIdeaResponse<SubscriptionServiceModel>.Failure(ErrorCode.CommandValidationFailure);
        }

        SubscriptionService subscriptionService = command.ToEntity();
        var response = await _subscriptionServicesService.AddAsync(subscriptionService, cancellationToken);
        var handlerResponse = GetHandlerResponse<SubscriptionService, SubscriptionServiceModel>(response, ErrorCode.EmptyEntityOnCreateSubscriptionService);

        return handlerResponse;
    }
}

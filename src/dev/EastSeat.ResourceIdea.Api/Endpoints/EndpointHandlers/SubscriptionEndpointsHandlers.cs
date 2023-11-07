using EastSeat.ResourceIdea.Application.Features.Subscription.Commands.CreateSubscription;
using EastSeat.ResourceIdea.Application.Features.Subscription.Queries.GetSubscriptionsList;
using EastSeat.ResourceIdea.Application.Responses;
using MediatR;

namespace EastSeat.ResourceIdea.Api.Endpoints.EndpointHandlers;

/// <summary>
/// Handles requests for the subscription resource.
/// </summary>
public static class SubscriptionEndpointsHandlers
{
    /// <summary>
    /// Gets the list of subscriptions.
    /// </summary>
    /// <param name="mediator"></param>
    /// <returns></returns>
    public static async Task<IResult> GetSubscriptionsAsync(IMediator mediator)
    {
        return TypedResults.Ok(await mediator.Send(new GetSubscriptionsListQuery()));
    }

    /// <summary>
    /// Adds a new subscription.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="createSubscriptionCommand"></param>
    /// <returns></returns>
    public static async Task<IResult> PostSubscriptionAsync(IMediator mediator, CreateSubscriptionCommand createSubscriptionCommand)
    {
        var commandResponse = await mediator.Send(createSubscriptionCommand);

        var response = new ApiResponse<CreateSubscriptionViewModel>(
            data: commandResponse.Subscription,
            success: commandResponse.Success,
            message: commandResponse.Message,
            errorCode: commandResponse.ErrorCode
        );

        return response.Success ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }
}

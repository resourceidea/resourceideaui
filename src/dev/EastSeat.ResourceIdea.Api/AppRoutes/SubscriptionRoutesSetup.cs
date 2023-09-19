using EastSeat.ResourceIdea.Application.Features.Subscription.Commands.CreateSubscription;
using EastSeat.ResourceIdea.Application.Features.Subscription.Queries.GetSubscriptionsList;
using EastSeat.ResourceIdea.Application.Responses;

using MediatR;

namespace EastSeat.ResourceIdea.Api.AppRoutes;

/// <summary>
/// Setup routes for subscription resource.
/// </summary>
public static class SubscriptionRoutesSetup
{
    public static WebApplication MapSubscriptionRoutes(this WebApplication app)
    {
        app.MapGet($"{Constants.ApiBaseRoutes.Subscriptions}", GetSubscriptionsAsync)
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest);

        app.MapPost($"{Constants.ApiBaseRoutes.Subscriptions}", PostSubscriptionAsync);

        return app;
    }

    private static async Task<IEnumerable<SubscriptionsListVM>> GetSubscriptionsAsync(IMediator mediator)
    {
        return await mediator.Send(new GetSubscriptionsListQuery());
    }

    private static async Task<IResult> PostSubscriptionAsync(IMediator mediator, CreateSubscriptionCommand createSubscriptionCommand)
    {
        var commandResponse = await mediator.Send(createSubscriptionCommand);

        var response = new ApiResponse<CreateSubscriptionViewModel>(
            data: commandResponse.Subscription,
            success: commandResponse.Success,
            message: commandResponse.Message,
            errorCode: commandResponse.ErrorCode
        );

        return response.Success ? Results.Ok(response) : Results.BadRequest(response);
    }
}

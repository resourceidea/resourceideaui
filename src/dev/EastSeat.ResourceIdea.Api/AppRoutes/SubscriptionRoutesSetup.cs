using EastSeat.ResourceIdea.Application.Features.Subscription.Commands.CreateSubscription;
using EastSeat.ResourceIdea.Application.Features.Subscription.Queries.GetSubscriptionsList;

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
           .Produces(StatusCodes.Status200OK);

        app.MapPost($"{Constants.ApiBaseRoutes.Subscriptions}", PostSubscriptionAsync)
           .Produces(StatusCodes.Status201Created);

        return app;
    }

    private static async Task<IEnumerable<SubscriptionsListVM>> GetSubscriptionsAsync(IMediator mediator)
    {
        return await mediator.Send(new GetSubscriptionsListQuery());
    }

    private static async Task<CreateSubscriptionViewModel> PostSubscriptionAsync(IMediator mediator, CreateSubscriptionCommand createSubscriptionCommand)
    {
        var response = await mediator.Send(createSubscriptionCommand);

        return response.Subscription;
    }
}

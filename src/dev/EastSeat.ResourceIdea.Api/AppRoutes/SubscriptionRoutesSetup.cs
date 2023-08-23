using EastSeat.ResourceIdea.Application.Features.Asset.Queries.GetAssetsList;
using EastSeat.ResourceIdea.Application.Features.Subscription.Commands.CreateSubscription;
using EastSeat.ResourceIdea.Application.Features.Subscription.Queries.GetSubscriptionsList;
using EastSeat.ResourceIdea.Domain.Constants;

using MediatR;

namespace EastSeat.ResourceIdea.Api.AppRoutes;

/// <summary>
/// Setup routes for subscription resource.
/// </summary>
public static class SubscriptionRoutesSetup
{
    public static WebApplication MapSubscriptionRoutes(this WebApplication app)
    {
        app.MapGet($"{StringConstants.SubscriptionsApiRoute}", GetSubscriptionsAsync)
           .Produces(StatusCodes.Status200OK);

        app.MapPost($"{StringConstants.SubscriptionsApiRoute}", PostSubscriptionAsync)
           .Produces(StatusCodes.Status201Created);

        return app;
    }

    private static async Task<IEnumerable<SubscriptionsListVM>> GetSubscriptionsAsync(IMediator mediator)
    {
        return await mediator.Send(new GetSubscriptionsListQuery());
    }

    private static async Task<CreateSubscriptionVM> PostSubscriptionAsync(IMediator mediator, CreateSubscriptionCommand createSubscriptionCommand)
    {
        var response = await mediator.Send(createSubscriptionCommand);

        return response.Subscription;
    }
}

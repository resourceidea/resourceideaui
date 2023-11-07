using EastSeat.ResourceIdea.Api.Endpoints.EndpointHandlers;

namespace EastSeat.ResourceIdea.Api.AppRoutes;

/// <summary>
/// Setup routes for subscription resource.
/// </summary>
public static class SubscriptionsEndpoints
{
    /// <summary>
    /// Maps subscriptions endpoints.
    /// </summary>
    /// <param name="app"></param>
    public static void MapSubscriptionEndpoints(this WebApplication app)
    {
        app.MapGet($"{Constants.ApiBaseRoutes.Subscriptions}", SubscriptionEndpointsHandlers.GetSubscriptionsAsync)
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest);

        app.MapPost($"{Constants.ApiBaseRoutes.Subscriptions}", SubscriptionEndpointsHandlers.PostSubscriptionAsync);
    }
}

namespace EastSeat.ResourceIdea.Api.AppRoutes;

public static class AppEndpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => "ResourceIdea API");

        app.MapAuthEndpoints();
        app.MapAssetsEndpoints();
        app.MapSubscriptionEndpoints();
    }
}

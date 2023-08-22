namespace EastSeat.ResourceIdea.Api.AppRoutes;

public static class AppRoutesSetup
{
    public static WebApplication MapRoutes(this WebApplication app)
    {
        app.MapGet("/", () => "Hello World!");

        app.MapAssetRoutes();

        return app;
    }
}

using EastSeat.ResourceIdea.Application.Features.Asset.Queries.GetAssetsList;

using MediatR;

namespace EastSeat.ResourceIdea.Api.AppRoutes;

/// <summary>
/// Setup routes to the assets endpoints.
/// </summary>
public static class AssetRoutesSetup
{
    public static WebApplication MapAssetRoutes(this WebApplication app)
    {
        app.MapGet("/assets", async (IMediator mediator) => {
            return await mediator.Send(new GetAssetsListQuery());
        })
        .Produces(StatusCodes.Status200OK);

        return app;
    }
}

using EastSeat.ResourceIdea.Application.Features.Asset.Commands;
using EastSeat.ResourceIdea.Application.Features.Asset.Queries.GetAssetsList;
using EastSeat.ResourceIdea.Domain.Constants;

using MediatR;

namespace EastSeat.ResourceIdea.Api.AppRoutes;

/// <summary>
/// Setup routes to the assets endpoints.
/// </summary>
public static class AssetRoutesSetup
{
    public static WebApplication MapAssetRoutes(this WebApplication app)
    {
        app.MapGet(StringConstants.AssetsApiRoute, GetAssetsAsync)
           .Produces(StatusCodes.Status200OK);

        app.MapPost(StringConstants.AssetsApiRoute, PostAssetAsync)
           .Produces(StatusCodes.Status201Created);

        return app;
    }

    private static async Task<List<AssetListVM>> GetAssetsAsync(IMediator mediator)
    {
        return await mediator.Send(new GetAssetsListQuery());
    }

    private static async Task<CreateAssetDTO> PostAssetAsync(IMediator mediator, CreateAssetCommand createAssetCommand)
    {
        var response = await mediator.Send(createAssetCommand);

        return response.Asset;
    }
}

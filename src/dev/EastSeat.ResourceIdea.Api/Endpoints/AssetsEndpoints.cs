using EastSeat.ResourceIdea.Api.Endpoints.EndpointHandlers;
using EastSeat.ResourceIdea.Application.Features.Asset.Commands;
using EastSeat.ResourceIdea.Application.Features.Asset.Queries.GetAssetsList;

using MediatR;

namespace EastSeat.ResourceIdea.Api.AppRoutes;

/// <summary>
/// Setup routes to the assets endpoints.
/// </summary>
public static class AssetsEndpoints
{
    /// <summary>
    /// Map assets endpoints.
    /// </summary>
    /// <param name="app"></param>
    public static void MapAssetsEndpoints(this WebApplication app)
    {
        app.MapGet(Constants.ApiBaseRoutes.Assets.GetAssets, AssetsEndpointHandlers.GetAssetsAsync)
           .Produces(StatusCodes.Status200OK);

        app.MapPost(Constants.ApiBaseRoutes.Assets.PostAsset, AssetsEndpointHandlers.PostAssetAsync)
           .Produces(StatusCodes.Status201Created);
    }
}

using EastSeat.ResourceIdea.Application.Features.Asset.Commands;
using EastSeat.ResourceIdea.Application.Features.Asset.Queries.GetAssetsList;
using MediatR;

namespace EastSeat.ResourceIdea.Api.Endpoints.EndpointHandlers;

/// <summary>
/// Handlers for the Assets endpoints
/// </summary>
public static class AssetsEndpointHandlers
{
    /// <summary>
    /// Handles getting assets.
    /// </summary>
    /// <param name="mediator"></param>
    /// <returns></returns>
    public static async Task<IResult> GetAssetsAsync(IMediator mediator)
    {
        return TypedResults.Ok(await mediator.Send(new GetAssetsListQuery()));
    }

    /// <summary>
    /// Add an asset.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="createAssetCommand"></param>
    /// <returns></returns>
    public static async Task<IResult> PostAssetAsync(IMediator mediator, CreateAssetCommand createAssetCommand)
    {
        return TypedResults.Ok(await mediator.Send(createAssetCommand));
    }
}

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Asset.Queries.GetAssetsList;

/// <summary>
/// Query to get a list of assets.
/// </summary>
public class GetAssetsListQuery : IRequest<List<AssetListVM>>
{
}

using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Asset.Queries.GetAssetsList;

/// <summary>
/// Handles the query to list assets.
/// </summary>
public class GetAssetsListQueryHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Asset> assetRepository) : IRequestHandler<GetAssetsListQuery, List<AssetListVM>>
{
    private readonly IAsyncRepository<Domain.Entities.Asset> assetRepository = assetRepository;
    private readonly IMapper mapper = mapper;

    /// <inheritdoc/>
    public async Task<List<AssetListVM>> Handle(GetAssetsListQuery request, CancellationToken cancellationToken)
    {
        var assets = await assetRepository.ListAllAsync();

        return mapper.Map<IEnumerable<AssetListVM>>(assets.OrderBy(a => a.Description)).ToList();
    }
}

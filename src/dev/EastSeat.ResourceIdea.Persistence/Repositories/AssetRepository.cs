using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Persistence.Repositories;

/// <summary>
/// Repository for the Asset records.
/// </summary>
public class AssetRepository : BaseRepository<Asset>, IAssetRepository
{
    public AssetRepository(ResourceIdeaDbContext dbContext) : base(dbContext)
    {
    }
}

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Persistence.Repositories;

/// <summary>
/// Repository for the asset assignment records.
/// </summary>
public class AssetAssignmentRepository(ResourceIdeaDbContext dbContext) : BaseRepository<AssetAssignment>(dbContext), IAssetAssignmentRepository
{
}

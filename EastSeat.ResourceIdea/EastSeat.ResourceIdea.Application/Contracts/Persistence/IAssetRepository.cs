using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Application.Contracts.Persistence;

/// <summary>
/// Asset repository interface.
/// </summary>
public interface IAssetRepository : IAsyncRepository<Asset>
{
}

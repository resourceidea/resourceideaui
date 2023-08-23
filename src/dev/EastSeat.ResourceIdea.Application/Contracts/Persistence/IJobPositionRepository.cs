using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Application.Contracts.Persistence;

/// <summary>
/// Job position repository.
/// </summary>
public interface IJobPositionRepository : IAsyncRepository<JobPosition>
{
}

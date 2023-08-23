using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Application.Contracts.Persistence;

/// <summary>
/// Assignment repository.
/// </summary>
public interface IAssignmentRepository : IAsyncRepository<Assignment>
{
}

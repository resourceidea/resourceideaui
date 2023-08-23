using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Application.Contracts.Persistence;

/// <summary>
/// Employee assignment repository.
/// </summary>
public interface IEmployeeAssignmentRepository : IAsyncRepository<EmployeeAssignment>
{
}

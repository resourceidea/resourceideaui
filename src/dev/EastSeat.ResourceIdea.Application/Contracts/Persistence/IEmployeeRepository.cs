using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Application.Contracts.Persistence;

/// <summary>
/// Employee repository.
/// </summary>
public interface IEmployeeRepository : IAsyncRepository<Employee>
{
}

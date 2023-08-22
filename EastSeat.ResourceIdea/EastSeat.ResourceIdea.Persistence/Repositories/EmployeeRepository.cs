using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Persistence.Repositories;

/// <summary>
/// Repository for the employee records.
/// </summary>
public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ResourceIdeaDbContext dbContext) : base(dbContext)
    {
    }
}

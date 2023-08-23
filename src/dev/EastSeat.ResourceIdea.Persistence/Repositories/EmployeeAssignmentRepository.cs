using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Persistence.Repositories;

/// <summary>
/// Repository for the employee assignment records.
/// </summary>
public class EmployeeAssignmentRepository : BaseRepository<EmployeeAssignment>, IEmployeeAssignmentRepository
{
    public EmployeeAssignmentRepository(ResourceIdeaDbContext dbContext) : base(dbContext)
    {
    }
}

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Persistence.Repositories;

/// <summary>
/// Repository for the employee assignment records.
/// </summary>
public class EmployeeAssignmentRepository(ResourceIdeaDbContext dbContext) : BaseRepository<EmployeeAssignment>(dbContext), IEmployeeAssignmentRepository
{
}

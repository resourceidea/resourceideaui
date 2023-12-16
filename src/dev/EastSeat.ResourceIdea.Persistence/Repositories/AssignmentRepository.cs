using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Persistence.Repositories;

/// <summary>
/// Repository for the assignments records.
/// </summary>
public class AssignmentRepository(ResourceIdeaDbContext dbContext) : BaseRepository<Assignment>(dbContext), IAssignmentRepository
{
}

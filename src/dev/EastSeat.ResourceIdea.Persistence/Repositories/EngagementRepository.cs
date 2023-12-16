using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Persistence.Repositories;

/// <summary>
/// Repository for the engagement records.
/// </summary>
public class EngagementRepository(ResourceIdeaDbContext dbContext) : BaseRepository<Engagement>(dbContext), IEngagementRepository
{
}

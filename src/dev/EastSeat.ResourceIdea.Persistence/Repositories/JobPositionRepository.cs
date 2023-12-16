using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Persistence.Repositories;

/// <summary>
/// Repository for the job position records.
/// </summary>
public class JobPositionRepository(ResourceIdeaDbContext dbContext) : BaseRepository<JobPosition>(dbContext), IJobPositionRepository
{
}

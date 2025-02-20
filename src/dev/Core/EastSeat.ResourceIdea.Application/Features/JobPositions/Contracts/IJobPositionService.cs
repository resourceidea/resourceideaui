// ----------------------------------------------------------------------------
// File: IJobPositionService.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\JobPositions\Contracts\IJobPositionService.cs
// Description: Defines the contract for a job position service.
// ----------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;

namespace EastSeat.ResourceIdea.Application.Features.JobPositions.Contracts;

/// <summary>
/// Defines the contract for a job position service.
/// </summary>
public interface IJobPositionService : IDataStoreService<JobPosition>
{
}
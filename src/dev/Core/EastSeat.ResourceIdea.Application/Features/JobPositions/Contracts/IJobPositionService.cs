// ----------------------------------------------------------------------------
// File: IJobPositionService.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\JobPositions\Contracts\IJobPositionService.cs
// Description: Defines the contract for a job position service.
// ----------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.JobPositions.Contracts;

/// <summary>
/// Defines the contract for a job position service.
/// </summary>
public interface IJobPositionService : IDataStoreService<JobPosition>
{
}
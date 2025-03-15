// ----------------------------------------------------------------------------
// File: IJobPositionService.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\JobPositions\Contracts\IJobPositionService.cs
// Description: Defines the contract for a job position service.
// ----------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.JobPositions.Contracts;

/// <summary>
/// Defines the contract for a job position service.
/// </summary>
public interface IJobPositionService : IDataStoreService<JobPosition>
{
    /// <summary>
    /// Get job positions by department ID.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="departmentId"></param>
    /// <param name="tenantId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ResourceIdeaResponse<PagedListResponse<JobPositionSummary>>> GetDepartmentJobPositionsAsync(
        int page,
        int size,
        TenantId tenantId,
        DepartmentId departmentId,
        CancellationToken cancellationToken);
}
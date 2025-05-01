// ===================================================================================
// File: GetJobPositionsByDepartmentIdQueryHandler.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\JobPositions\Handlers\GetJobPositionsByDepartmentIdQueryHandler.cs
// Description: Query handler to get job positions by department ID.
// ===================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Contracts;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Queries;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.Types;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.JobPositions.Handlers;

public sealed class GetJobPositionsByDepartmentIdQueryHandler(IJobPositionService jobPositionService)
    : BaseHandler,
      IRequestHandler<GetJobPositionsByDepartmentIdQuery, ResourceIdeaResponse<PagedListResponse<JobPositionSummary>>>
{
    private readonly IJobPositionService _jobPositionService = jobPositionService;

    /// <summary>
    /// Handles the query to get job positions by department ID.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Paged list of job positions.</returns>
    public async Task<ResourceIdeaResponse<PagedListResponse<JobPositionSummary>>> Handle(
        GetJobPositionsByDepartmentIdQuery query,
        CancellationToken cancellationToken) =>
        await _jobPositionService.GetDepartmentJobPositionsAsync(
            query.PageNumber,
            query.PageSize,
            query.TenantId,
            query.DepartmentId,
            cancellationToken);
}
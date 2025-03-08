// ----------------------------------------------------------------------------------
// File: GetAllJobPositionsQueryHandler.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\JobPositions\Handlers\GetAllJobPositionsQueryHandler.cs
// Description: Query handler for retrieving all job positions with department information.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Contracts;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Queries;
using EastSeat.ResourceIdea.Application.Features.Departments.Contracts;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.Types;
using MediatR;
using EastSeat.ResourceIdea.Domain.Departments.Entities;
using EastSeat.ResourceIdea.Application.Mappers;

namespace EastSeat.ResourceIdea.Application.Features.JobPositions.Handlers;

/// <summary>
/// Handler for retrieving all job positions with department information.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GetAllJobPositionsQueryHandler"/> class.
/// </remarks>
/// <param name="jobPositionService">The job position service.</param>
/// <param name="departmentService">The department service.</param>
public sealed class GetAllJobPositionsQueryHandler(
    IJobPositionService jobPositionService)
    : BaseHandler,
      IRequestHandler<GetAllJobPositionsQuery, ResourceIdeaResponse<PagedListResponse<TenantJobPositionModel>>>
{
    private readonly IJobPositionService _jobPositionService = jobPositionService;

    /// <summary>
    /// Handles the specified query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of job positions with department information.</returns>
    public async Task<ResourceIdeaResponse<PagedListResponse<TenantJobPositionModel>>> Handle(
        GetAllJobPositionsQuery query,
        CancellationToken cancellationToken)
    {
        var tenantIdSpec = new TenantIdSpecification<JobPosition>(query.TenantId);
        var jobPositionsResponse = await _jobPositionService.GetPagedListAsync(
            query.PageNumber,
            query.PageSize,
            tenantIdSpec,
            cancellationToken);

        if (!jobPositionsResponse.IsSuccess || !jobPositionsResponse.Content.HasValue)
        {
            return ResourceIdeaResponse<PagedListResponse<TenantJobPositionModel>>.Failure(jobPositionsResponse.Error);
        }

        return jobPositionsResponse.ToResourceIdeaResponse<TenantJobPositionModel>();
    }
}
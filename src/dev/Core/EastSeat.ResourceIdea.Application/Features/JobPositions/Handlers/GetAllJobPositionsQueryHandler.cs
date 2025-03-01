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

namespace EastSeat.ResourceIdea.Application.Features.JobPositions.Handlers;

/// <summary>
/// Handler for retrieving all job positions with department information.
/// </summary>
public sealed class GetAllJobPositionsQueryHandler (IJobPositionService jobPositionService): BaseHandler, 
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
        var specification = new TenantIdSpecification<JobPosition>(query.TenantId);
        var jobPositionsQueryResponse = await _jobPositionService.GetPagedListAsync(
            query.PageNumber,
            query.PageSize,
            specification,
            cancellationToken);
        
        return GetHandlerResponse<JobPosition, TenantJobPositionModel>(jobPositionsQueryResponse);
    }
}
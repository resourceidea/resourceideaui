using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Contracts;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Queries;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Specifications;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.Types;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.JobPositions.Handlers;

public sealed class GetJobPositionsByDepartmentIdQueryHandler(IJobPositionService jobPositionService)
    : BaseHandler,
      IRequestHandler<GetJobPositionsByDepartmentIdQuery, ResourceIdeaResponse<PagedListResponse<JobPositionModel>>>
{
    private readonly IJobPositionService _jobPositionService = jobPositionService;

    /// <summary>
    /// Handles the query to get job positions by department ID.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Paged list of job positions.</returns>
    public async Task<ResourceIdeaResponse<PagedListResponse<JobPositionModel>>> Handle(
        GetJobPositionsByDepartmentIdQuery query,
        CancellationToken cancellationToken)
    {
        BaseSpecification<JobPosition> specification = new TenantIdSpecification<JobPosition>(query.TenantId)
            .And(new JobPositionDepartmentIdSpecification(query.DepartmentId));

        var response = await _jobPositionService.GetPagedListAsync(
                query.PageNumber,
                query.PageSize,
                specification,
                cancellationToken);

        return GetHandlerResponse<JobPosition, JobPositionModel>(response);
    }
}
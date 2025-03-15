// ------------------------------------------------------------------------------
// File: GetJobPositionByIdQueryHandler.cs
// Path: src/dev/Core/EastSeat.ResourceIdea.Application/Features/JobPositions/Handlers/GetJobPositionByIdQueryHandler.cs
// Description: Handler for GetJobPositionByIdQuery
// ------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Contracts;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Queries;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Specifications;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.Types;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.JobPositions.Handlers;

/// <summary>
/// Handler for GetJobPositionByIdQuery
/// </summary>
public sealed class GetJobPositionByIdQueryHandler(IJobPositionService jobPositionService)
    : BaseHandler,
      IRequestHandler<GetJobPositionByIdQuery, ResourceIdeaResponse<JobPositionModel>>
{
    private readonly IJobPositionService _jobPositionService = jobPositionService;

    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<JobPositionModel>> Handle(
        GetJobPositionByIdQuery request,
        CancellationToken cancellationToken)
    {
        // Create specification for the job position with the given ID
        var specification = new JobPositionByIdSpecification(request.JobPositionId, request.TenantId);
        
        // Get the job position using the specification
        var response = await _jobPositionService.GetByIdAsync(specification, cancellationToken);
        if (response.IsFailure)
        {
            return ResourceIdeaResponse<JobPositionModel>.Failure(response.Error);
        }
        
        if (response.Content.HasValue is false)
        {
            return ResourceIdeaResponse<JobPositionModel>.NotFound();
        }
        
        // Map the entity to model and return
        var jobPositionModel = response.Content.Value.ToModel<JobPositionModel>();
        return ResourceIdeaResponse<JobPositionModel>.Success(Optional<JobPositionModel>.Some(jobPositionModel));
    }
}
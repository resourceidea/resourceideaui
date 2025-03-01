// ------------------------------------------------------------------------------
// File: UpdateJobPositionCommandHandler.cs
// Path: src/dev/Core/EastSeat.ResourceIdea.Application/Features/JobPositions/Handlers/UpdateJobPositionCommandHandler.cs
// Description: Handler for UpdateJobPositionCommand
// ------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Commands;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Contracts;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Specifications;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.Types;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.JobPositions.Handlers;

/// <summary>
/// Handler for UpdateJobPositionCommand
/// </summary>
public sealed class UpdateJobPositionCommandHandler(IJobPositionService jobPositionService)
    : BaseHandler,
      IRequestHandler<UpdateJobPositionCommand, ResourceIdeaResponse<JobPositionModel>>
{
    private readonly IJobPositionService _jobPositionService = jobPositionService;

    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<JobPositionModel>> Handle(
        UpdateJobPositionCommand command,
        CancellationToken cancellationToken)
    {
        // First, verify the job position exists
        var specification = new JobPositionByIdSpecification(command.Id, command.TenantId);
        var jobPositionQueryResponse = await _jobPositionService.GetByIdAsync(specification, cancellationToken);
        if (jobPositionQueryResponse.IsFailure)
        {
            return ResourceIdeaResponse<JobPositionModel>.Failure(jobPositionQueryResponse.Error);
        }

        if (!jobPositionQueryResponse.Content.HasValue)
        {
            return ResourceIdeaResponse<JobPositionModel>.NotFound();
        }
        
        var existingJobPosition = jobPositionQueryResponse.Content.Value;
        existingJobPosition.Title = command.Title;
        existingJobPosition.Description = command.Description;
        existingJobPosition.LastModified = DateTimeOffset.UtcNow;
        
        var response = await _jobPositionService.UpdateAsync(existingJobPosition, cancellationToken);
        if (response.IsFailure)
        {
            return ResourceIdeaResponse<JobPositionModel>.Failure(response.Error);
        }
               
        if (response.Content.HasValue is false)
        {
            return ResourceIdeaResponse<JobPositionModel>.Failure(ErrorCode.EmptyEntityOnUpdateJobPosition);
        }
        
        return response.Content.Value.ToResourceIdeaResponse<JobPosition, JobPositionModel>();
    }
}
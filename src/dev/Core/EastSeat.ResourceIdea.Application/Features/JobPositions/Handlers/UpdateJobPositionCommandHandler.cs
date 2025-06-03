// ------------------------------------------------------------------------------
// File: UpdateJobPositionCommandHandler.cs
// Path: src/dev/Core/EastSeat.ResourceIdea.Application/Features/JobPositions/Handlers/UpdateJobPositionCommandHandler.cs
// Description: Handler for UpdateJobPositionCommand
// ------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Commands;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Contracts;
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
        var validationResponse = command.Validate();
        if (validationResponse.IsValid is false)
        {
            // TODO: Log the validation errors.
            return ResourceIdeaResponse<JobPositionModel>.Failure(ErrorCode.BadRequest);
        }

        JobPosition jobPositionUpdate = command.ToEntity();
        var response = await _jobPositionService.UpdateAsync(jobPositionUpdate, cancellationToken);
        if (response.IsFailure)
        {
            return ResourceIdeaResponse<JobPositionModel>.Failure(response.Error);
        }

        if (response.Content is null)
        {
            return ResourceIdeaResponse<JobPositionModel>.Failure(ErrorCode.EmptyEntityOnUpdateJobPosition);
        }

        return response.Content.ToResourceIdeaResponse<JobPosition, JobPositionModel>();
    }
}
// ------------------------------------------------------------------
// File: CreateJobPositionCommandHandler.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\JobPositions\Handlers\CreateJobPositionCommandHandler.cs
// Description: Command handler for creating a job position.
// ------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Handlers;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Commands;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Contracts;
using EastSeat.ResourceIdea.Application.Features.Tenants.Contracts;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.Types;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.JobPositions.Handlers;

/// <summary>
/// Command handler for creating a job position.
/// </summary>
/// <param name="jobPositionService"></param>
/// <param name="tenantsService"></param>
public sealed class CreateJobPositionCommandHandler(IJobPositionService jobPositionService)
    : BaseHandler, IRequestHandler<CreateJobPositionCommand, ResourceIdeaResponse<JobPositionModel>>
{
    private readonly IJobPositionService _jobPositionService = jobPositionService;

    public async Task<ResourceIdeaResponse<JobPositionModel>> Handle(
        CreateJobPositionCommand command,
        CancellationToken cancellationToken)
    {
        JobPosition newJobPosition = command.ToEntity();
        ResourceIdeaResponse<JobPosition> response = await _jobPositionService.AddAsync(
            newJobPosition,
            cancellationToken);

        if (response is { IsSuccess: false } || response.Content.HasValue == false)
        {
            return ResourceIdeaResponse<JobPositionModel>.Failure(ErrorCode.DbInsertFailureOnCreateJobPosition);
        }

        var responseModel = response.Content.Value.ToModel<JobPositionModel>();

        return ResourceIdeaResponse<JobPositionModel>.Success(Optional<JobPositionModel>.Some(responseModel));
    }
}
using AutoMapper;
using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Handlers;

public sealed class AssignEngagementTaskCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<AssignEngagementTaskCommand, ResourceIdeaResponse<EngagementTaskModel>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<EngagementTaskModel>> Handle(
        AssignEngagementTaskCommand request,
        CancellationToken cancellationToken)
    {
        using (_unitOfWork)
        {
            _unitOfWork.BeginTransaction();

            var result = await AssignTaskAsync(_unitOfWork, request.EngagementTaskId, request.ApplicationUserId, cancellationToken);
            
            var response = result.Match(
                onFailure: HandleTaskAssignmentFailure,
                onSuccess: HandleTaskAssignmentSuccess
            );
            
            if (!response.Success)
            {
                _unitOfWork.Rollback();
                
                return response;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _unitOfWork.Commit();

            return response;

        }
    }

    private static async Task<Either<ErrorCode, Optional<EngagementTaskAssignment>>> AssignTaskAsync(
        IUnitOfWork unitOfWork,
        EngagementTaskId engagementTaskId,
        ApplicationUserId applicationUserId,
        CancellationToken cancellationToken)
    {
        var iAsyncRepository = unitOfWork.GetRepository<EngagementTaskAssignment>();
        if (iAsyncRepository is IEngagementTaskAssignmentRepository engagementTaskAssignmentRepository)
        {
            return new SuccessResponse<ErrorCode, Optional<EngagementTaskAssignment>>(
                await engagementTaskAssignmentRepository.AssignAsync(engagementTaskId, applicationUserId, cancellationToken)
            );
        }

        return new FailureResponse<ErrorCode, Optional<EngagementTaskAssignment>>(ErrorCode.GetRepositoryFailure);
    }

    private static ResourceIdeaResponse<EngagementTaskModel> HandleTaskAssignmentFailure(ErrorCode errorCode) => new()
    {
        Success = false,
        ErrorCode = errorCode.ToString()
    };

    private ResourceIdeaResponse<EngagementTaskModel> HandleTaskAssignmentSuccess(Optional<EngagementTaskAssignment> engagementTaskAssignment)
    {
        var repository = _unitOfWork.GetRepository<EngagementTask>();
        var preconditionsCheckResult = MeetsPreconditions(repository, engagementTaskAssignment);
        if (preconditionsCheckResult.Success is false)
        {
            return new ResourceIdeaResponse<EngagementTaskModel>
            {
                Success = false,
                ErrorCode = preconditionsCheckResult.ErrorCode.ToString()
            };
        }
        
        var updatedEngagementTask = preconditionsCheckResult.Repository.SetAssignmentStatusFlagAsync(
            engagementTaskAssignment.Value.EngagementTaskId,
            CancellationToken.None);

        return new ResourceIdeaResponse<EngagementTaskModel>
        {
            Success = true,
            Content = Optional<EngagementTaskModel>.Some(_mapper.Map<EngagementTaskModel>(updatedEngagementTask))
        };
    }

    private static PreconditionsCheckResult MeetsPreconditions(
        IAsyncRepository<EngagementTask> repository,
        Optional<EngagementTaskAssignment> engagementTaskAssignment)
    {
        if (repository is not IEngagementTaskRepository engagementTaskRepository)
        {
            // If the repository is not an instance of IEngagementTaskRepository, then the repository is not valid.
            return new PreconditionsCheckResult(false, ErrorCode.GetRepositoryFailure, null!);
        }

        if (engagementTaskAssignment.HasValue is false)
        {
            // If the engagement task assignment is not present, then the entity is empty.
            return new PreconditionsCheckResult(false, ErrorCode.EmptyEntity, null!);
        }
        
        return new PreconditionsCheckResult(true, ErrorCode.None, engagementTaskRepository);
    }

    private record PreconditionsCheckResult(bool Success, ErrorCode ErrorCode, IEngagementTaskRepository Repository);
}
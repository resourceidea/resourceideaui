
using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Handlers;
public sealed class UpdateEngagementTaskCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateEngagementTaskCommand, ResourceIdeaResponse<EngagementTaskModel>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<EngagementTaskModel>> Handle(
        UpdateEngagementTaskCommand request,
        CancellationToken cancellationToken)
    {
        using (_unitOfWork)
        {
            var repository = _unitOfWork.GetRepository<EngagementTask>();
            if (repository is not IAsyncRepository<EngagementTask> engagementTaskRepository)
            {
                return new ResourceIdeaResponse<EngagementTaskModel>
                {
                    Success = false,
                    Message = "Repository not implemented.",
                    ErrorCode = "REPOSITORY_NOT_IMPLEMENTED",
                };
            }

            _unitOfWork.BeginTransaction();

                EngagementTask engagementTaskUpdate = new()
                {
                    Id = request.EngagementTaskId,
                    Description = request.Description,
                    Title = request.Title,
                    DueDate = request.DueDate,
                    EngagementId = request.EngagementId,
                };

                var engagementTaskUpdateResult = await repository.UpdateAsync(engagementTaskUpdate, cancellationToken);

                EngagementTask engagementTask = engagementTaskUpdateResult.Match(
                    some: engagementTask => engagementTask,
                    none: () => EmptyEngagementTask.Instance);

                if (engagementTask == EmptyEngagementTask.Instance)
                {
                    _unitOfWork.Rollback();

                    return new ResourceIdeaResponse<EngagementTaskModel>
                    {
                        Success = false,
                        Message = "Engagement Task not found.",
                        ErrorCode = "ENGAGEMENT_TASK_NOT_FOUND",
                    };
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _unitOfWork.Commit();

                return new ResourceIdeaResponse<EngagementTaskModel>
                {
                    Success = true,
                    Content = _mapper.Map<EngagementTaskModel>(engagementTask),
                };
        }
    }
}
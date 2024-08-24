
using AutoMapper;
using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
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
                return ResourceIdeaResponse<EngagementTaskModel>.Failure(ErrorCode.GetRepositoryFailure);
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
            if (engagementTaskUpdateResult.IsFailure)
            {
                _unitOfWork.Rollback();
                return ResourceIdeaResponse<EngagementTaskModel>.Failure(engagementTaskUpdateResult.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _unitOfWork.Commit();

            return ResourceIdeaResponse<EngagementTaskModel>
                        .Success(_mapper.Map<EngagementTaskModel>(engagementTaskUpdateResult.Content.Value));
        }
    }
}
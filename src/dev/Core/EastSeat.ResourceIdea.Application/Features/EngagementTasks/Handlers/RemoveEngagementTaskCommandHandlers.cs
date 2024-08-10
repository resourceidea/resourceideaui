using System;
using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Handlers;

/// <summary>
/// Handles the command to remove an engagement task.
/// </summary>
public class RemoveEngagementTaskCommandHandlers(IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveEngagementTaskCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(RemoveEngagementTaskCommand request, CancellationToken cancellationToken)
    {
        using (_unitOfWork)
        {
            var specification = new GetEngagementTaskByIdSpecification(request.EngagementTaskId);
            var repository = _unitOfWork.GetRepository<EngagementTask>();
            if (repository is not IAsyncRepository<EngagementTask> engagementTaskRepository)
            {
                _unitOfWork.BeginTransaction();

                var engagementTaskQueryResult = await repository.GetByIdAsync(specification, cancellationToken);
                EngagementTask engagementTask = engagementTaskQueryResult.Match(
                    some: engagementTask => engagementTask,
                    none: () => EmptyEngagementTask.Instance);

                if (engagementTask == EmptyEngagementTask.Instance)
                {
                    // TODO: Log failure to find engagement task for removal.
                    _unitOfWork.Rollback();
                    return;
                }

                engagementTask.Status = EngagementTaskStatus.Removed;
                await repository.UpdateAsync(engagementTask, cancellationToken);

                await repository.DeleteAsync(engagementTask);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _unitOfWork.Commit();
            }
        }
    }
}

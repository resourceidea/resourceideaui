using System;
using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Specifications;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Handlers;

/// <summary>
/// Handles the command to remove an engagement task.
/// </summary>
public class RemoveEngagementTaskCommandHandlers(IEngagementTaskRepository repository)
    : IRequestHandler<RemoveEngagementTaskCommand>
{
    private readonly IEngagementTaskRepository _repository = repository;

    public async Task Handle(RemoveEngagementTaskCommand request, CancellationToken cancellationToken)
    {
        var specification = new GetEngagementTaskByIdSpecification(request.EngagementTaskId);
        var engagementTaskQueryResult = await _repository.GetByIdAsync(specification, cancellationToken);
        EngagementTask engagementTask = engagementTaskQueryResult.Match(
            some: engagementTask => engagementTask,
            none: () => EmptyEngagementTask.Instance);
        
        if (engagementTask == EmptyEngagementTask.Instance)
        {
            // TODO: Log failure to find engagement task for removal.
            return;
        }

        await _repository.DeleteAsync(engagementTask);
    }
}

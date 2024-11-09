using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Handlers;

/// <summary>
/// Handles the command to create an engagement task.
/// </summary>
/// <param name="engagementTasksService"></param>
/// <param name="mapper"></param>
public class CreateEngagementTaskCommandHandler (
    IEngagementTasksService engagementTasksService,
    IMapper mapper) : IRequestHandler<CreateEngagementTaskCommand, ResourceIdeaResponse<EngagementTaskModel>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IEngagementTasksService _engagementTasksService = engagementTasksService;

    public async Task<ResourceIdeaResponse<EngagementTaskModel>> Handle(CreateEngagementTaskCommand request, CancellationToken cancellationToken)
    {
        var engagementTask = new EngagementTask
        {
            Id = EngagementTaskId.Create(Guid.NewGuid()),
            Description = request.Description,
            Title = request.Title,
            EngagementId = request.EngagementId,
            IsAssigned = false,
            Status = EngagementTaskStatus.NotStarted,
            DueDate = request.DueDate
        };

        var createdEngagementTask = await _engagementTasksService.AddAsync(engagementTask, cancellationToken);

        return ResourceIdeaResponse<EngagementTaskModel>
                    .Success(Optional<EngagementTaskModel>.Some(_mapper.Map<EngagementTaskModel>(createdEngagementTask)));
    }
}
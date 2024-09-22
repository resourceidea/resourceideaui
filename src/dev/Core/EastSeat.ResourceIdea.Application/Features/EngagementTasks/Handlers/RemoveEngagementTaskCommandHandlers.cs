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
public sealed class RemoveEngagementTaskCommandHandlers(
    IEngagementTasksService engagementTasksService,
    IMapper mapper) : IRequestHandler<RemoveEngagementTaskCommand, ResourceIdeaResponse<EngagementTaskModel>>
{
    private readonly IEngagementTasksService _engagementTasksService = engagementTasksService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<EngagementTaskModel>> Handle(RemoveEngagementTaskCommand request, CancellationToken cancellationToken)
    {
        // TODO: Validate the command.
        var result = await _engagementTasksService.RemoveAsync(request.EngagementTaskId, cancellationToken);
        return _mapper.Map<ResourceIdeaResponse<EngagementTaskModel>>(result);
    }
}

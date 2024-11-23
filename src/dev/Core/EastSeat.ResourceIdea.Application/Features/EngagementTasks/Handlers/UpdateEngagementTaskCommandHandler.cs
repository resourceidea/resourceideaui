
using AutoMapper;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Handlers;
public sealed class UpdateEngagementTaskCommandHandler(
    IEngagementTasksService engagementTasksService, IMapper mapper)
    : IRequestHandler<UpdateEngagementTaskCommand, ResourceIdeaResponse<EngagementTaskModel>>
{
    private readonly IEngagementTasksService _engagementTasksService = engagementTasksService;
    private readonly IMapper _mapper = mapper;

    public async Task<ResourceIdeaResponse<EngagementTaskModel>> Handle(
        UpdateEngagementTaskCommand request,
        CancellationToken cancellationToken)
    {
        // TODO: Map UpdateEngagementTaskCommand to the EngagementTask.
        var engagementTask = _mapper.Map<EngagementTask>(request);
        var response = await _engagementTasksService.UpdateAsync(engagementTask, cancellationToken);

        return _mapper.Map<ResourceIdeaResponse<EngagementTaskModel>>(response);
    }
}
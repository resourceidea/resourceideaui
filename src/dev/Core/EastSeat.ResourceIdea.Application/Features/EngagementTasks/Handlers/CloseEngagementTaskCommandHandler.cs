using AutoMapper;

using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Commands;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Models;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Handlers;

public sealed class CloseEngagementTaskCommandHandler (
    IEngagementTasksService engagementTasksService,
    IMapper mapper) : IRequestHandler<CloseEngagementTaskCommand, ResourceIdeaResponse<EngagementTaskModel>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IEngagementTasksService _engagementTasksService = engagementTasksService;

    public async Task<ResourceIdeaResponse<EngagementTaskModel>> Handle(CloseEngagementTaskCommand request, CancellationToken cancellationToken)
    {
        var result = await _engagementTasksService.CloseAsync(request.EngagementTaskId, cancellationToken);

        // TODO: Add error handling for the result to return the error on the result.
        if (result.IsFailure)
        {
            return ResourceIdeaResponse<EngagementTaskModel>.Failure(result.Error);
        }

        return _mapper.Map<ResourceIdeaResponse<EngagementTaskModel>>(result);
    }
}
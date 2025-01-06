using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers;

public sealed class CreateEngagementCommandHandler (IEngagementsService engagementsService)
    : IRequestHandler<CreateEngagementCommand, ResourceIdeaResponse<EngagementModel>>
{
    private readonly IEngagementsService _engagementsService = engagementsService;
    
    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<EngagementModel>> Handle(
        CreateEngagementCommand request,
        CancellationToken cancellationToken)
    {
        Engagement engagement = request.ToEntity();
        var result = await _engagementsService.AddAsync(engagement, cancellationToken);
        if (result.IsFailure)
        {
            return ResourceIdeaResponse<EngagementModel>.Failure(result.Error);
        }

        if (result.Content.HasValue is false)
        {
            return ResourceIdeaResponse<EngagementModel>.Failure(ErrorCode.EmptyEntityOnCreateEngagement);
        }

        return result.Content.Value.ToResourceIdeaResponse();
    }
}

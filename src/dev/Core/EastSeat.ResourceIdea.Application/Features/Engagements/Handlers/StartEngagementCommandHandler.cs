using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Features.Engagements.Validators;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers;

public sealed class StartEngagementCommandHandler (IEngagementsService engagementsService)
    : IRequestHandler<StartEngagementCommand, ResourceIdeaResponse<EngagementModel>>
{
    private readonly IEngagementsService _engagementsService = engagementsService;
    
    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<EngagementModel>> Handle(
        StartEngagementCommand request,
        CancellationToken cancellationToken)
    {
        StartEngagementCommandValidator startEngagementValidator = new();
        var validationResult = startEngagementValidator.Validate(request);
        
        if (validationResult.IsValid is false || validationResult.Errors.Count > 0)
        {
            return ResourceIdeaResponse<EngagementModel>.Failure(ErrorCode.StartEngagementCommandValidationFailure);
        }

        ResourceIdeaResponse<Engagement> response = await _engagementsService.StartAsync(
            request.EngagementId,
            request.CommencementDate,
            cancellationToken);
        if (response.IsFailure)
        {
            return ResourceIdeaResponse<EngagementModel>.Failure(response.Error);
        }

        if (response.Content != null is false)
        {
            return ResourceIdeaResponse<EngagementModel>.Failure(ErrorCode.EmptyEntityOnStartEngagement);
        }

        return response.Content.ToResourceIdeaResponse();
    }
}

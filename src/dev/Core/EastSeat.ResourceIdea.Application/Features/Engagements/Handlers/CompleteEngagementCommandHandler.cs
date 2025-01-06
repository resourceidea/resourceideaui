using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Features.Engagements.Validators;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers;

public sealed class CompleteEngagementCommandHandler (IEngagementsService engagementsService): IRequestHandler<CompleteEngagementCommand, ResourceIdeaResponse<EngagementModel>>
{
    private readonly IEngagementsService _engagementsService = engagementsService;

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<EngagementModel>> Handle(
        CompleteEngagementCommand request,
        CancellationToken cancellationToken)
    {
        CompleteEngagementCommandValidator completeEngagementValidator = new();
        var validationResult = completeEngagementValidator.Validate(request);

        if (!validationResult.IsValid || validationResult.Errors.Count > 0)
        {
            return ResourceIdeaResponse<EngagementModel>.Failure(ErrorCode.DataStoreCommandFailure);
        }

        ResourceIdeaResponse<Engagement> result = await _engagementsService.CompleteAsync(
            request.EngagementId,
            request.CompletionDate,
            cancellationToken);
        if (result.IsFailure)
        {
            return ResourceIdeaResponse<EngagementModel>.Failure(result.Error);
        }

        if (result.Content.HasValue is false)
        {
            return ResourceIdeaResponse<EngagementModel>.Failure(ErrorCode.EmptyEntityOnCompleteEngagement);
        }

        return result.Content.Value.ToResourceIdeaResponse();
    }
}

using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers
{
    /// <summary>
    /// Handles the cancellation of an engagement.
    /// </summary>
    public sealed class CancelEngagementCommandHandler(IEngagementsService engagementsService) : IRequestHandler<CancelEngagementCommand, ResourceIdeaResponse<EngagementModel>>
    {
        private readonly IEngagementsService _engagementsService = engagementsService;

        /// <inheritdoc />
        public async Task<ResourceIdeaResponse<EngagementModel>> Handle(CancelEngagementCommand request, CancellationToken cancellationToken)
        {
            ResourceIdeaResponse<Engagement> result = await _engagementsService.CancelAsync(request.EngagementId, cancellationToken);
            if (result.IsFailure)
            {
                return ResourceIdeaResponse<EngagementModel>.Failure(result.Error);
            }

            if (result.Content is null)
            {
                return ResourceIdeaResponse<EngagementModel>.Failure(ErrorCode.EmptyEntityOnCancelEngagement);
            }

            return result.Content.ToResourceIdeaResponse();
        }
    }
}

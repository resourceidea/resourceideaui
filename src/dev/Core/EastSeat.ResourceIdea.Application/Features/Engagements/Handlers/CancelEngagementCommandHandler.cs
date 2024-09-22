using AutoMapper;

using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers
{
    /// <summary>
    /// Handles the cancellation of an engagement.
    /// </summary>
    public sealed class CancelEngagementCommandHandler(
        IEngagementsService engagementsService,
        IMapper mapper) : IRequestHandler<CancelEngagementCommand, ResourceIdeaResponse<EngagementModel>>
    {
        private readonly IEngagementsService _engagementsService = engagementsService;
        private readonly IMapper _mapper = mapper;

        /// <inheritdoc />
        public async Task<ResourceIdeaResponse<EngagementModel>> Handle(CancelEngagementCommand request, CancellationToken cancellationToken)
        {
            var canceledEngagement = await _engagementsService.CancelAsync(request.EngagementId, cancellationToken);

            return ResourceIdeaResponse<EngagementModel>.Success(_mapper.Map<EngagementModel>(canceledEngagement));
        }
    }
}

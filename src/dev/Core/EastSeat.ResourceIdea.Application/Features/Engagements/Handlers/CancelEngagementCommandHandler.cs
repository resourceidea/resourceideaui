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
    public sealed class CancelEngagementCommandHandler (
        IEngagementRepository engagementRepository,
        IMapper mapper) : IRequestHandler<CancelEngagementCommand, ResourceIdeaResponse<EngagementModel>>
    {
        private readonly IEngagementRepository _engagementRepository = engagementRepository;
        private readonly IMapper _mapper = mapper;

        /// <inheritdoc />
        public async Task<ResourceIdeaResponse<EngagementModel>> Handle(CancelEngagementCommand request, CancellationToken cancellationToken)
        {
            Engagement engagement = new()
            {
                Id = request.EngagementId
            };

            var canceledEngagement = await _engagementRepository.CancelAsync(engagement, cancellationToken);

            return new ResourceIdeaResponse<EngagementModel>
            {
                Success = true,
                Message = $"Engagement canceled successfully.",
                Content = Optional<EngagementModel>.Some(_mapper.Map<EngagementModel>(canceledEngagement))
            };
        }
    }
}

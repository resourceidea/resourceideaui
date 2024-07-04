using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers
{
    /// <summary>
    /// Handles the cancellation of an engagement.
    /// </summary>
    public sealed class CancelEngagementCommandHandler : IRequestHandler<CancelEngagementCommand, ResourceIdeaResponse<EngagementModel>>
    {
        /// <inheritdoc />
        public async Task<ResourceIdeaResponse<EngagementModel>> Handle(CancelEngagementCommand request, CancellationToken cancellationToken)
        {
            // TODO: Validate cancel engagement command.

            // TODO: Execute cancel engagement command.

            // TODO: Return response.
        }
    }
}

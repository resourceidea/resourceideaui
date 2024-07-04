using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;

/// <summary>
/// Represents the interface for the engagement repository.
/// </summary>
public interface IEngagementRepository : IAsyncRepository<Engagement>
{
}

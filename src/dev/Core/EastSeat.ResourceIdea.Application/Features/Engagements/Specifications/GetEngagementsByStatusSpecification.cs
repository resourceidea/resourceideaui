using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Enums;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Specifications;

/// <summary>
/// Specification to retrieve an engagement by their status.
/// </summary>
/// <param name="status">Engagement status.</param>
public class GetEngagementsByStatusSpecification(EngagementStatus status) : BaseSpecification<Engagement>
{
    private readonly EngagementStatus _status = status;

    /// <summary>
    /// Criteria to retrieve an engagements by their status.
    /// </summary>
    public override Expression<Func<Engagement, bool>> Criteria => engagement => engagement.EngagementStatus == _status;
}
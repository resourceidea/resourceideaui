using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Specifications;

/// <summary>
/// Specification to retrieve an engagement by its identifier.
/// </summary>
/// <param name="engagementId">Engagement identifier.</param>
public class GetEngagementByIdSpecification(EngagementId engagementId) : BaseSpecification<Engagement>
{
    private readonly EngagementId _engagementId = engagementId;

    /// <summary>
    /// Criteria to retrieve an engagement by its identifier.
    /// </summary>
    public override Expression<Func<Engagement, bool>> Criteria => engagement => engagement.Id == _engagementId;
}
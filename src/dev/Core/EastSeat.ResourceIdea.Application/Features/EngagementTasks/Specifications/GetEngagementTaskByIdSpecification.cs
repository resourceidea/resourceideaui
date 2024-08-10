using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.EngagementTasks.Specifications;

/// <summary>
/// Specification to retrieve an engagement task by its identifier.
/// </summary>
/// <param name="engagementTaskId">Engagement task identifier.</param>
public class GetEngagementTaskByIdSpecification(EngagementTaskId engagementTaskId) : BaseSpecification<EngagementTask>
{
    private readonly EngagementTaskId _engagementTaskId = engagementTaskId;

    /// <summary>
    /// Criteria to retrieve an engagement task by its identifier.
    /// </summary>
    public override Expression<Func<EngagementTask, bool>> Criteria => engagement => engagement.Id == _engagementTaskId;
}
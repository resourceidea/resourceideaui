using EastSeat.ResourceIdea.Domain.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.Enums;

namespace EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;

public sealed record EmptyEngagementTask
{
    /// <summary>
    /// Instance of an empty <see cref="Engagement"/>.
    /// </summary>
    public static EngagementTask Instance => new()
    {
        Id = EngagementTaskId.Empty,
        EngagementId = EngagementId.Empty,
        DueDate = EmptyDate.Value,
        Description = string.Empty,
        Title = string.Empty,
        Status = EngagementTaskStatus.NotStarted,
        Assigned = false        
    };

    private EmptyEngagementTask() { }
}

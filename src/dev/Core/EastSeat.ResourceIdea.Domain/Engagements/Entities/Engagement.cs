using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Domain.Engagements.Entities;

/// <summary>
/// Engagement entity class.
/// </summary>
public class Engagement : BaseEntity
{
    /// <summary>
    /// DepartmentId of an engagement.
    /// </summary>
    public EngagementId Id { get; set; }

    /// <summary>
    /// Description of the engagement.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// DepartmentId of the client that the engagement is for.
    /// </summary>
    public ClientId ClientId { get; set; }

    /// <summary>
    /// Date when the engagement work is started.
    /// </summary>
    public DateTimeOffset? CommencementDate { get; set; }

    /// <summary>
    /// Date when the engagement work is completed.
    /// </summary>
    public DateTimeOffset? CompletionDate { get; set; }

    /// <summary>
    /// Status of the engagement.
    /// </summary>
    public EngagementStatus EngagementStatus { get; set; }

    /// <summary>
    /// Engagement tasks associated with the engagement.
    /// </summary>
    public IReadOnlyCollection<EngagementTask>? EngagementTasks { get; set; }

    public override TModel ToModel<TModel>()
    {
        throw new NotImplementedException();
    }

    public override ResourceIdeaResponse<TModel> ToResourceIdeaResponse<TEntity, TModel>()
    {
        throw new NotImplementedException();
    }
}

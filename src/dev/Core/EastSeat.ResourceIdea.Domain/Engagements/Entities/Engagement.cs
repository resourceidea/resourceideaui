using EastSeat.ResourceIdea.Domain.Clients.Entities;
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

    /// <summary>Client associated with the engagement.</summary>
    public Client? Client { get; set; }

    public override TModel ToModel<TModel>()
    {
        // Only EngagementModel is supported for now
        if (typeof(TModel) == typeof(Models.EngagementModel))
        {
            var model = new Models.EngagementModel
            {
                Id = Id,
                ClientId = ClientId,
                TenantId = TenantId,
                CommencementDate = CommencementDate,
                CompletionDate = CompletionDate,
                Status = EngagementStatus,
                Description = Description ?? string.Empty,
                ClientName = Client?.Name ?? string.Empty
            };
            return (TModel)(object)model;
        }
        throw new InvalidOperationException($"Cannot map {nameof(Engagement)} to {typeof(TModel).Name}");
    }

    public override ResourceIdeaResponse<TModel> ToResourceIdeaResponse<TEntity, TModel>()
    {
        // Only EngagementModel is supported for now
        if (typeof(TModel) == typeof(Models.EngagementModel))
        {
            return ResourceIdeaResponse<TModel>.Success(ToModel<TModel>());
        }
        throw new InvalidOperationException($"Cannot map {typeof(TEntity).Name} to {typeof(TModel).Name}");
    }
}

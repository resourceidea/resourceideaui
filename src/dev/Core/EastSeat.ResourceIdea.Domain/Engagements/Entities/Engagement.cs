using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
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
    /// Title of the engagement.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of the engagement.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// DepartmentId of the client that the engagement is for.
    /// </summary>
    public ClientId ClientId { get; set; }

    /// <summary>
    /// Date when the engagement is to start.
    /// </summary>
    public DateTimeOffset? StartDate { get; set; }

    /// <summary>
    /// Date when the engagement is to end.
    /// </summary>
    public DateTimeOffset? EndDate { get; set; }

    /// <summary>
    /// Status of the engagement.
    /// </summary>
    public EngagementStatus EngagementStatus { get; set; }

    /// <summary>
    /// Id of the manager responsible for the engagement.
    /// </summary>
    public EmployeeId? ManagerId { get; set; }

    /// <summary>
    /// Id of the partner responsible for the engagement.
    /// </summary>
    public EmployeeId? PartnerId { get; set; }

    public string? Color { get; set; }

    /// <summary>
    /// Migration project id for the engagement, if applicable.
    /// </summary>
    public string? MigrationProjectId { get; set; }

    /// <summary>
    /// Migration client id for the engagement, if applicable.
    /// </summary>
    public string? MigrationClientId { get; set; }

    /// <summary>
    /// Migration job id for the engagement, if applicable.
    /// </summary>
    public string? MigrationJobId { get; set; }

    /// <summary>
    /// Migration manager for the engagement, if applicable.
    /// </summary>
    public string? MigrationManager { get; set; }

    /// <summary>
    /// Migration partner for the engagement, if applicable.
    /// </summary>
    public string? MigrationPartner { get; set; }

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
                Title = Title,
                ClientId = ClientId,
                TenantId = TenantId,
                StartDate = StartDate,
                EndDate = EndDate,
                Status = EngagementStatus,
                Description = Description ?? string.Empty,
                ClientName = Client?.Name ?? string.Empty,
                ManagerId = ManagerId,
                PartnerId = PartnerId
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

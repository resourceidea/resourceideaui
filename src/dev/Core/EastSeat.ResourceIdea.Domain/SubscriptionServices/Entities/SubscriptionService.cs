using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.SubscriptionServices.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Domain.SubscriptionServices.Entities;

/// <summary>
/// Subscribed service entity.
/// </summary>
public class SubscriptionService : BaseEntity
{
    /// <summary>Service DepartmentId.</summary>
    public SubscriptionServiceId Id { get; set; }

    /// <summary>Service name.</summary>
    public string Name { get; set; } = string.Empty;

    public override TModel ToModel<TModel>()
    {
        throw new NotImplementedException();
    }

    public override ResourceIdeaResponse<TModel> ToResourceIdeaResponse<TEntity, TModel>()
    {
        throw new NotImplementedException();
    }
}
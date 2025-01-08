using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Domain.Tenants.Entities;

/// <summary>
/// Tenant entity.
/// </summary>
public class Tenant : BaseEntity
{
    /// <summary>
    /// Tenant's organization name.
    /// </summary>
    public string Organization { get; set; } = string.Empty;

    public override TModel ToModel<TModel>()
    {
        throw new NotImplementedException();
    }

    public override ResourceIdeaResponse<TModel> ToResourceIdeaResponse<TEntity, TModel>()
    {
        throw new NotImplementedException();
    }
}
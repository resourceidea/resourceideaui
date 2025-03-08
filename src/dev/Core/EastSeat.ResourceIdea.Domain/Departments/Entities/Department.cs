using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Departments.Models;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Domain.Departments.Entities;

/// <summary>
/// Department entity.
/// </summary>
public class Department : BaseEntity
{
    /// <summary>
    /// Department ID.
    /// </summary>
    public DepartmentId Id { get; set; }

    /// <summary>
    /// Department name.
    /// </summary>
    public required string Name { get; set; }

    public override TModel ToModel<TModel>()
    {
        if (typeof(TModel) == typeof(DepartmentModel))
        {
            return (TModel)(object)new DepartmentModel
            {
                DepartmentId = Id,
                Name = Name
            };
        }

        throw new InvalidOperationException($"Mapping for {typeof(TModel).Name} is not configured.");
    }

    public override ResourceIdeaResponse<TModel> ToResourceIdeaResponse<TEntity, TModel>()
    {
        throw new NotImplementedException();
    }
}

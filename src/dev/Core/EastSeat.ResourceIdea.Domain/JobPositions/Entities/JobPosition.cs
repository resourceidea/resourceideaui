// ----------------------------------------------------------------------------------
// File: JobPosition.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\JobPositions\Entities\JobPosition.cs
// Description: JobPosition entity.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Domain.JobPositions.Entities;

/// <summary>
/// JobPosition entity.
/// </summary>
public class JobPosition : BaseEntity
{
    /// <summary>
    /// JobPosition ID.
    /// </summary>
    public JobPositionId Id { get; set; }

    /// <summary>
    /// JobPosition name.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// JobPosition description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Department ID.
    /// </summary>
    public DepartmentId DepartmentId { get; set; }

    /// <summary>
    /// Maps the entity to a model.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <returns>TModel</returns>
    public override TModel ToModel<TModel>()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Maps the entity to a ResourceIdeaResponse.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    /// <returns></returns>
    public override ResourceIdeaResponse<TModel> ToResourceIdeaResponse<TEntity, TModel>()
    {
        throw new NotImplementedException();
    }
}
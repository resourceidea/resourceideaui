// ----------------------------------------------------------------------------------
// File: JobPosition.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\JobPositions\Entities\JobPosition.cs
// Description: JobPosition entity.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
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
    public override TModel ToModel<TModel>() where TModel : class
    {
        return typeof(TModel) switch
        {
            var t when t == typeof(JobPositionModel) => (TModel)(object)MapToJobPositionModel(),
            _ => throw new InvalidOperationException($"Mapping for {typeof(TModel).Name} is not configured."),
        };
    }

    /// <summary>
    /// Maps this entity to a <see cref="ResourceIdeaResponse{TModel} />.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    /// <returns><see cref="ResourceIdeaResponse{TModel}"/> instance.</returns>
    public override ResourceIdeaResponse<TModel> ToResourceIdeaResponse<TEntity, TModel>()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Maps this entity to a <see cref="JobPositionModel"/>.
    /// </summary>
    /// <returns><see cref="JobPositionModel"/> instance.</returns>
    private JobPositionModel MapToJobPositionModel() =>
        new()
        {
            Id = Id,
            Title = Title ?? string.Empty,
            Description = Description ?? string.Empty,
            DepartmentId = DepartmentId
        };
}
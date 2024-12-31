using EastSeat.ResourceIdea.Domain.Departments.Entities;
using EastSeat.ResourceIdea.Domain.Departments.Models;

namespace EastSeat.ResourceIdea.Domain.Departments.Mappers;

public static class DepartmentMapper
{
    /// <summary>
    /// Map department entity to the department model.
    /// </summary>
    /// <typeparam name="TModel">Department model</typeparam>
    /// <param name="department">Department entity</param>
    /// <returns>Mapped department model</returns>
    /// <exception cref="InvalidCastException">Thrown when the casting to department model fails.</exception>
    /// <exception cref="NotSupportedException">Thrown when the mapping is not supported.</exception>
    public static TModel ToModel<TModel>(this Department department) where TModel : BaseDepartmentModel
    {
        return typeof(TModel) switch
        {
            _ when typeof(TModel) == typeof(DepartmentListModel) => ToDepartmentListModel(department) as TModel ?? throw new InvalidCastException(),
            _ when typeof(TModel) == typeof(DepartmentUpdateModel) => ToDepartmentUpdateModel(department) as TModel ?? throw new InvalidCastException(),
            _ => throw new NotSupportedException($"Mapping to type {typeof(TModel).Name} is not supported.")
        };
    }

    /// <summary>
    /// Map <see cref="DepartmentCreateModel"/> to <see cref="Department"/>.
    /// </summary>
    /// <param name="model">Model to be mapped to the entity.</param>
    /// <returns>Instance of <see cref="Department"/></returns>
    public static Department ToEntity(this DepartmentCreateModel model)
    {
        return new Department
        {
            Name = model.Name,
            TenantId = model.TenantId
        };
    }

    /// <summary>
    /// Map <see cref="DepartmentUpdateModel"/> to <see cref="Department"/> entity.
    /// </summary>
    /// <param name="model">Model to be mapped to the entity.</param>
    /// <returns>Instance of <see cref="Department"/></returns>
    public static Department ToEntity(this DepartmentUpdateModel model)
    {
        return new Department
        {
            Id = model.DepartmentId,
            Name = model.Name,
            TenantId = model.TenantId
        };
    }

    private static DepartmentListModel ToDepartmentListModel(Department department)
    {
        return new DepartmentListModel
        {
            DepartmentId = department.Id,
            Name = department.Name,
            TenantId = department.TenantId,
            IsDeleted = department.IsDeleted
        };
    }

    private static DepartmentUpdateModel ToDepartmentUpdateModel(Department department)
    {
        return new DepartmentUpdateModel
        {
            DepartmentId = department.Id,
            Name = department.Name,
            TenantId = department.TenantId
        };
    }
}

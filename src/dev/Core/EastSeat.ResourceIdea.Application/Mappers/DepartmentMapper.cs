using EastSeat.ResourceIdea.Application.Extensions;
using EastSeat.ResourceIdea.Domain.Departments.Entities;
using EastSeat.ResourceIdea.Domain.Departments.Models;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Mappers;

/// <summary>
/// Mapper for the department entity and models.
/// </summary>
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
    public static TModel ToModel<TModel>(this Department department) where TModel : class
    {
        return typeof(TModel) switch
        {
            _ when typeof(TModel) == typeof(DepartmentViewModel) => ToDepartmentViewModel(department) as TModel ?? throw new InvalidCastException(),
            _ => throw new NotSupportedException($"Mapping to type {typeof(TModel).Name} is not supported.")
        };
    }

    /// <summary>
    /// Map <see cref="DepartmentViewModel"/> to <see cref="Department"/> entity.
    /// </summary>
    /// <param name="model">Model to be mapped to the entity.</param>
    /// <returns>Instance of <see cref="Department"/></returns>
    public static Department ToEntity(this DepartmentViewModel model)
    {
        return new Department
        {
            Id = model.DepartmentId,
            Name = model.Name,
            TenantId = model.TenantId
        };
    }

    /// <summary>
    /// Map <see cref="IEnumerable{Department}"/> to <see cref="DepartmentListModel"/>.
    /// </summary>
    /// <param name="departments">Collection of department entities.</param>
    /// <returns>Instance of <see cref="DepartmentListModel"/></returns>
    public static DepartmentListModel ToDepartmentListModel(this IEnumerable<Department> departments)
    {
        var departmentViewModels = departments.Select(department => department.ToModel<DepartmentViewModel>()).ToList();
        return new DepartmentListModel(departmentViewModels);
    }

    public static ResourceIdeaResponse<DepartmentListModel> ToResourceIdeaResponse(this IEnumerable<Department> departments)
    {
        DepartmentListModel departmentListModel = ToDepartmentListModel(departments);
        return ResourceIdeaResponse<DepartmentListModel>.Success(departmentListModel);
    }

    /// <summary>
    /// Map <see cref="Department"/> entity to <see cref="ResourceIdeaResponse{DepartmentViewModel}"/>.
    /// </summary>
    /// <param name="department">Department entity</param>
    /// <returns>Instance of <see cref="ResourceIdeaResponse{DepartmentViewModel}"/></returns>
    public static ResourceIdeaResponse<DepartmentViewModel> ToResourceIdeaResponse(this Department department)
    {
        return ResourceIdeaResponse<DepartmentViewModel>.Success(ToDepartmentViewModel(department));
    }

    private static DepartmentViewModel ToDepartmentViewModel(Department department)
    {
        ArgumentNullException.ThrowIfNull(department);
        department.Name.ThrowIfNullOrEmptyOrWhiteSpace();

        return new DepartmentViewModel
        {
            DepartmentId = department.Id,
            Name = department.Name,
            TenantId = department.TenantId
        };
    }
}

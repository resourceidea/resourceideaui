// ====================================================================================
// File: Employee.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\Employees\Entities\Employee.cs
// Description: Employee entity.
// ====================================================================================

using System.ComponentModel.DataAnnotations.Schema;
using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Employees.Entities;

/// <summary>
/// Employee entity.
/// </summary>
public class Employee : BaseEntity
{
    public EmployeeId EmployeeId { get; set; }

    public JobPositionId JobPositionId { get; set; }

    public ApplicationUserId ApplicationUserId { get; set; }

    public string? EmployeeNumber { get; set; }

    public EmployeeId ReportsTo { get; set; }

    public string? MigrationResourceId { get; set; }

    public JobPosition? JobPosition { get; set; }

    public DateTimeOffset? HireDate { get; set; }

    public DateTimeOffset? EndDate { get; set; }

    public string? MigrationUserId { get; set; }

    public string? MigrationFullname { get; set; }

    public string? MigrationCompanyCode { get; set; }

    public string? MigrationJobPositionId { get; set; }

    public string? MigrationJobsManagedColor { get; set; }

    [NotMapped]
    public string FirstName { get; set; } = string.Empty;

    [NotMapped]
    public string LastName { get; set; } = string.Empty;

    [NotMapped]
    public string Email { get; set; } = string.Empty;

    /// <inheritdoc/>
    public override TModel ToModel<TModel>() where TModel : class
    {
        return typeof(TModel) switch
        {
            var t when t == typeof(EmployeeModel) => (TModel)(object)MapToEmployeeModel(),
            var t when t == typeof(TenantEmployeeModel) => (TModel)(object)MapToTenantEmployeeModel(),
            _ => throw new InvalidOperationException($"Mapping for {typeof(TModel).Name} is not configured."),
        };
    }

    /// <inheritdoc/>
    public override ResourceIdeaResponse<TModel> ToResourceIdeaResponse<TEntity, TModel>()
    {
        return typeof(TModel) switch
        {
            var t when t == typeof(EmployeeModel) => ResourceIdeaResponse<TModel>.Success(ToModel<TModel>()),
            var t when t == typeof(TenantEmployeeModel) => ResourceIdeaResponse<TModel>.Success(ToModel<TModel>()),
            _ => throw new InvalidOperationException($"Cannot map {typeof(TEntity).Name} to {typeof(TModel).Name}")
        };
    }

    private EmployeeModel MapToEmployeeModel() => new()
    {
        EmployeeId = EmployeeId,
        JobPositionId = JobPositionId,
        ApplicationUserId = ApplicationUserId,
        EmployeeNumber = EmployeeNumber ?? string.Empty,
        ManagerId = ReportsTo,
        FirstName = FirstName,
        LastName = LastName,
        Email = Email,
        DepartmentId = JobPosition?.Department?.Id ?? DepartmentId.Empty,
        TenantId = TenantId,
    };

    private TenantEmployeeModel MapToTenantEmployeeModel() => new()
    {
        EmployeeId = EmployeeId,
        FirstName = FirstName,
        LastName = LastName,
        Email = Email,
        JobPositionTitle = JobPosition?.Title ?? string.Empty,
        DepartmentName = JobPosition?.Department?.Name ?? string.Empty,
    };
}

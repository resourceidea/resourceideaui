// ============================================================================================
// File: PotentialManagersSpecification.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Specifications\PotentialManagersSpecification.cs
// Description: Specification for retrieving potential managers for an employee
// ============================================================================================

using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Specifications;

/// <summary>
/// Specification for retrieving potential managers for an employee.
/// Returns all employees in the same tenant except the specified employee.
/// </summary>
/// <param name="tenantId">The tenant ID.</param>
/// <param name="excludeEmployeeId">The employee ID to exclude from results.</param>
public class PotentialManagersSpecification(TenantId tenantId, EmployeeId excludeEmployeeId) : BaseSpecification<Employee>
{
    public TenantId TenantId => tenantId;
    public EmployeeId ExcludeEmployeeId => excludeEmployeeId;

    public override Expression<Func<Employee, bool>> Criteria =>
        employee => employee.TenantId == tenantId && employee.EmployeeId != excludeEmployeeId;
}
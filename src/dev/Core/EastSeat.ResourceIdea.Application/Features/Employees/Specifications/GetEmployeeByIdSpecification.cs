// ======================================================================================
// File: GetTenantEmployeeByIdQuery.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Queries\GetTenantEmployeeByIdQuery.cs
// Description: Query to get a tenant employee by ID.
// ======================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using System.Linq.Expressions;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Specifications;

/// <summary>
/// Specification for retrieving an employee by its ID
/// </summary>
public class GetEmployeeIdBySpecification(EmployeeId employeeId, TenantId tenantId) : BaseSpecification<Employee>
{
    public EmployeeId EmployeeId => employeeId;

    public TenantId TenantId => tenantId;

    /// <summary>
    /// Gets the criteria to filter employees by ID
    /// </summary>
    public override Expression<Func<Employee, bool>> Criteria =>
        e => e.EmployeeId == EmployeeId
          && e.TenantId == TenantId
          && (e.EndDate == null || e.EndDate > DateTimeOffset.Now);
}
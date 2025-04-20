// ======================================================================================
// File: GetTenantEmployeeByIdQuery.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Queries\GetTenantEmployeeByIdQuery.cs
// Description: Query to get a tenant employee by ID.
// ======================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using System.Linq.Expressions;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Specifications;

/// <summary>
/// Specification for retrieving an employee by its ID
/// </summary>
public class EmployeeIdBySpecification(EmployeeId employeeId) : BaseSpecification<Employee>
{
    public EmployeeId EmployeeId => employeeId;

    /// <summary>
    /// Gets the criteria to filter employees by ID
    /// </summary>
    public override Expression<Func<Employee, bool>> Criteria => e => e.EmployeeId == EmployeeId;
}
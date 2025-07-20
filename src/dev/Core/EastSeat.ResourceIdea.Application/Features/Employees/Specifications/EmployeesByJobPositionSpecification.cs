// ============================================================================================
// File: EmployeesByJobPositionSpecification.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Specifications\EmployeesByJobPositionSpecification.cs
// Description: Specification for retrieving employees by job position
// ============================================================================================

using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Specifications;

/// <summary>
/// Specification for retrieving employees by job position.
/// </summary>
/// <param name="tenantId">The tenant ID</param>
/// <param name="jobPositionId">The job position ID</param>
public class EmployeesByJobPositionSpecification(TenantId tenantId, JobPositionId jobPositionId) : BaseSpecification<Employee>
{
    public TenantId TenantId => tenantId;
    public JobPositionId JobPositionId => jobPositionId;

    public override Expression<Func<Employee, bool>> Criteria =>
        employee => employee.TenantId == tenantId && employee.JobPositionId == jobPositionId;
}
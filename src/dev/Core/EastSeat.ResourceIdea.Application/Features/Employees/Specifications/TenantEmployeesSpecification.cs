// ============================================================================================
// File: TenantEmployeesSpecification.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Specifications\TenantEmployeesSpecification.cs
// Description: Specification for retrieving tenant employees
// ============================================================================================

using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Specifications;

/// <summary>
/// Specification for retrieving tenant employees.
/// </summary>
/// <param name="tenantId"></param>
public class TenantEmployeesSpecification(TenantId tenantId) : BaseSpecification<Employee>
{
    public TenantId TenantId => tenantId;

    public override Expression<Func<Employee, bool>> Criteria =>
        employee => employee.TenantId == tenantId && 
                   (employee.EndDate == null || employee.EndDate > DateTimeOffset.Now);
}

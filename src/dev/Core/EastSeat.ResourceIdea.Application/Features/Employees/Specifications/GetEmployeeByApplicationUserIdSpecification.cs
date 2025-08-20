// ======================================================================================
// File: GetEmployeeByApplicationUserIdSpecification.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Specifications\GetEmployeeByApplicationUserIdSpecification.cs
// Description: Specification for retrieving an employee by ApplicationUserId.
// ======================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using System.Linq.Expressions;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Specifications;

/// <summary>
/// Specification for retrieving an employee by ApplicationUserId
/// </summary>
public class GetEmployeeByApplicationUserIdSpecification(ApplicationUserId applicationUserId, TenantId tenantId) : BaseSpecification<Employee>
{
    public ApplicationUserId ApplicationUserId => applicationUserId;

    public TenantId TenantId => tenantId;

    /// <summary>
    /// Gets the criteria to filter employees by ApplicationUserId
    /// </summary>
    public override Expression<Func<Employee, bool>> Criteria =>
        e => e.ApplicationUserId == ApplicationUserId
          && e.TenantId == TenantId
          && (e.EndDate == null || e.EndDate > DateTimeOffset.Now);
}
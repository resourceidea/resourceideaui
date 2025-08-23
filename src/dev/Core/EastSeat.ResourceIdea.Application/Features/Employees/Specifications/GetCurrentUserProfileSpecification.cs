// ======================================================================================
// File: GetCurrentUserProfileSpecification.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Specifications\GetCurrentUserProfileSpecification.cs
// Description: Specification for retrieving current user's profile information.
// ======================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using System.Linq.Expressions;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Specifications;

/// <summary>
/// Specification for retrieving current user's profile information by ApplicationUserId
/// </summary>
public class GetCurrentUserProfileSpecification(ApplicationUserId applicationUserId, TenantId tenantId) : BaseSpecification<Employee>
{
    public ApplicationUserId ApplicationUserId => applicationUserId;

    public TenantId TenantId => tenantId;

    /// <summary>
    /// Gets the criteria to filter employees by ApplicationUserId for profile
    /// </summary>
    public override Expression<Func<Employee, bool>> Criteria =>
        e => e.ApplicationUserId == ApplicationUserId
          && e.TenantId == TenantId
          && (e.EndDate == null || e.EndDate > DateTimeOffset.Now);
}
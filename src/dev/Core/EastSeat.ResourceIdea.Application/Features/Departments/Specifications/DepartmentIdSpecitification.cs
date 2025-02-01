using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Departments.Entities;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

using System.Linq.Expressions;

namespace EastSeat.ResourceIdea.Application.Features.Departments.Specifications;

/// <summary>
/// Specification used to get a department by ID and the tenant ID.
/// </summary>
/// <param name="departmentId"></param>
/// <param name="tenantId"></param>
public sealed class DepartmentIdSpecification(DepartmentId departmentId, TenantId tenantId) : BaseSpecification<Department>
{
    private readonly DepartmentId _departmentId = departmentId;
    private readonly TenantId _tenantId = tenantId;

    public override Expression<Func<Department, bool>> Criteria
        => department => department.Id == _departmentId && department.TenantId == _tenantId;
}

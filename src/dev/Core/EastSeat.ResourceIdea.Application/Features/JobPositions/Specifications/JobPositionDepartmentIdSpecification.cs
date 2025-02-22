// -------------------------------------------------------------------------------
// File: JobPositionDepartmentIdSpecification.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\JobPositions\Specifications\JobPositionDepartmentIdSpecification.cs
// Description: Specification to filter job positions by department ID.
// -------------------------------------------------------------------------------

using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;

namespace EastSeat.ResourceIdea.Application.Features.JobPositions.Specifications;

/// <summary>
/// Specification to filter job positions by department ID.
/// </summary>
/// <param name="departmentId"></param>
public sealed class JobPositionDepartmentIdSpecification(DepartmentId departmentId) : BaseSpecification<JobPosition>
{
    private readonly DepartmentId _departmentId = departmentId;

    public override Expression<Func<JobPosition, bool>> Criteria
        => jobPosition => jobPosition.DepartmentId == _departmentId;
}
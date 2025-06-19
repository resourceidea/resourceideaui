// ------------------------------------------------------------------------------
// File: JobPositionByIdSpecification.cs
// Path: src/dev/Core/EastSeat.ResourceIdea.Application/Features/JobPositions/Specifications/JobPositionByIdSpecification.cs
// Description: Specification for retrieving job position by ID
// ------------------------------------------------------------------------------

using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.JobPositions.Specifications;

/// <summary>
/// Specification for retrieving job position by ID.
/// </summary>
public class JobPositionByIdSpecification(JobPositionId jobPositionId, TenantId tenantId)
    : BaseSpecification<JobPosition>
{
    private readonly JobPositionId _jobPositionId = jobPositionId;
    private readonly TenantId _tenantId = tenantId;

    public override Expression<Func<JobPosition, bool>> Criteria
        => department => department.Id == _jobPositionId && department.TenantId == _tenantId;
}
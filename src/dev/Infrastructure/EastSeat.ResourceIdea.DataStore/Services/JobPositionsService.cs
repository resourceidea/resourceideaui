// --------------------------------------------------------------------------
// File: JobPositionsService.cs
// Path: src\dev\Infrastructure\EastSeat.ResourceIdea.DataStore\Services\JobPositionsService.cs
// Description: Service to perform CRUD operations on JobPosition entities.
// --------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.JobPositions.Contracts;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.Types;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EastSeat.ResourceIdea.DataStore.Services;

public sealed class JobPositionsService(ResourceIdeaDBContext dbContext) : IJobPositionService
{
    private readonly ResourceIdeaDBContext _dbContext = dbContext;

    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<JobPosition>> AddAsync(
        JobPosition entity,
        CancellationToken cancellationToken)
    {
        EntityEntry<JobPosition> result = await _dbContext.JobPositions.AddAsync(entity, cancellationToken);
        int changes = await _dbContext.SaveChangesAsync(cancellationToken);
        if (!JobPositionCreatedSuccessfully(result, changes))
        {
            return ResourceIdeaResponse<JobPosition>.Failure(ErrorCode.DbInsertFailureOnCreateJobPosition);
        }

        return ResourceIdeaResponse<JobPosition>.Success(Optional<JobPosition>.Some(result.Entity));
    }

    /// <inheritdoc/>
    public Task<ResourceIdeaResponse<JobPosition>> DeleteAsync(
        JobPosition entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<ResourceIdeaResponse<JobPosition>> GetByIdAsync(
        BaseSpecification<JobPosition> specification,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<ResourceIdeaResponse<PagedListResponse<JobPosition>>> GetPagedListAsync(
        int page,
        int size,
        Optional<BaseSpecification<JobPosition>> specification,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<ResourceIdeaResponse<JobPosition>> UpdateAsync(
        JobPosition entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private static bool JobPositionCreatedSuccessfully(
        EntityEntry<JobPosition> result,
        int changes)
        => result.Entity != null && changes > 0;
}
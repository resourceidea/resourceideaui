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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EastSeat.ResourceIdea.DataStore.Services;

/// <summary>
/// Service to perform CRUD operations on JobPosition entities.
/// </summary>
/// <param name="dbContext"></param>
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
    public async Task<ResourceIdeaResponse<JobPosition>> GetByIdAsync(
        BaseSpecification<JobPosition> specification,
        CancellationToken cancellationToken)
    {
        JobPosition? jobPosition = await _dbContext.JobPositions
                                                   .AsQueryable()
                                                   .Where(specification.Criteria)
                                                   .FirstOrDefaultAsync(cancellationToken);

        if (jobPosition == null)
        {
            return ResourceIdeaResponse<JobPosition>.NotFound();
        }

        return ResourceIdeaResponse<JobPosition>.Success(Optional<JobPosition>.Some(jobPosition));
    }

    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<PagedListResponse<JobPosition>>> GetPagedListAsync(
        int page,
        int size,
        Optional<BaseSpecification<JobPosition>> specification,
        CancellationToken cancellationToken)
    {
        IQueryable<JobPosition> query = _dbContext.JobPositions.AsQueryable();

        if (specification.HasValue)
        {
            query = query.Where(specification.Value.Criteria);
        }

        int totalCount = await query.CountAsync(cancellationToken);
        List<JobPosition> items = await query
                                      .Include(d => d.Department)
                                      .OrderBy(d => d.Title)
                                      .Skip((page - 1) * size)
                                      .Take(size)
                                      .ToListAsync(cancellationToken);

        PagedListResponse<JobPosition> pagedList = new()
        {
            Items = items,
            TotalCount = totalCount,
            CurrentPage = page,
            PageSize = size
        };

        return ResourceIdeaResponse<PagedListResponse<JobPosition>>
                .Success(Optional<PagedListResponse<JobPosition>>.Some(pagedList));
    }

    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<JobPosition>> UpdateAsync(
        JobPosition entity,
        CancellationToken cancellationToken)
    {
        var jobPosition = await _dbContext.JobPositions
            .FirstOrDefaultAsync(j => j.Id == entity.Id
                && j.TenantId == entity.TenantId
                && j.DepartmentId == entity.DepartmentId, cancellationToken);

        if (jobPosition == null)
        {
            return ResourceIdeaResponse<JobPosition>.NotFound();
        }

        jobPosition.Title = entity.Title;
        jobPosition.Description = entity.Description;
                
        _dbContext.JobPositions.Update(jobPosition);
        int changes = await _dbContext.SaveChangesAsync(cancellationToken);
        if (changes <= 0)
        {
            // TODO: Log failure to update department.
            return ResourceIdeaResponse<JobPosition>.Failure(ErrorCode.DbUpdateFailureOnUpdateDepartment);
        }

        return ResourceIdeaResponse<JobPosition>.Success(Optional<JobPosition>.Some(jobPosition));
    }

    private static bool JobPositionCreatedSuccessfully(
        EntityEntry<JobPosition> result,
        int changes)
        => result.Entity != null && changes > 0;
}
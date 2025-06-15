// =============================================================================================================
// File: WorkItemsService.cs
// Path: src\dev\Infrastructure\EastSeat.ResourceIdea.DataStore\Services\WorkItemsService.cs
// Description: This file contains the WorkItemsService class implementation.
// =============================================================================================================

using EastSeat.ResourceIdea.Application.Features.WorkItems.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EastSeat.ResourceIdea.DataStore.Services;

/// <summary>
/// Service for managing work items in the data store.
/// </summary>
/// <param name="dbContext">The database context.</param>
public sealed class WorkItemsService(ResourceIdeaDBContext dbContext) : IWorkItemsService
{
    private readonly ResourceIdeaDBContext _dbContext = dbContext;

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<WorkItem>> AddAsync(WorkItem entity, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.WorkItems.AddAsync(entity, cancellationToken);
            int result = await _dbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return ResourceIdeaResponse<WorkItem>.Success(entity);
            }

            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DatabaseError);
        }
        catch (DbUpdateException)
        {
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DatabaseError);
        }
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<WorkItem>> DeleteAsync(WorkItem entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<WorkItem>> GetByIdAsync(BaseSpecification<WorkItem> specification, CancellationToken cancellationToken)
    {
        WorkItem? workItem = await _dbContext.WorkItems.AsNoTracking().FirstOrDefaultAsync(specification.Criteria, cancellationToken);
        if (workItem == null)
        {
            return ResourceIdeaResponse<WorkItem>.NotFound();
        }

        return ResourceIdeaResponse<WorkItem>.Success(workItem);
    }

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<PagedListResponse<WorkItem>>> GetPagedListAsync(int page, int size, Optional<BaseSpecification<WorkItem>> specification, CancellationToken cancellationToken)
    {
        IQueryable<WorkItem> query = _dbContext.WorkItems.AsNoTracking();
        
        if (specification.HasValue)
        {
            query = query.Where(specification.Value.Criteria);
        }

        int totalCount = await query.CountAsync(cancellationToken);
        IReadOnlyList<WorkItem> workItems = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        var pagedListResponse = new PagedListResponse<WorkItem>
        {
            Items = workItems,
            TotalCount = totalCount,
            CurrentPage = page,
            PageSize = size,
        };

        return ResourceIdeaResponse<PagedListResponse<WorkItem>>.Success(pagedListResponse);
    }

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<WorkItem>> UpdateAsync(WorkItem entity, CancellationToken cancellationToken)
    {
        try
        {
            var existingWorkItem = await _dbContext.WorkItems.FirstOrDefaultAsync(w => w.Id == entity.Id, cancellationToken);
            if (existingWorkItem == null)
            {
                return ResourceIdeaResponse<WorkItem>.NotFound();
            }

            _dbContext.Entry(existingWorkItem).CurrentValues.SetValues(entity);
            int result = await _dbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return ResourceIdeaResponse<WorkItem>.Success(existingWorkItem);
            }
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DatabaseError);
        }
        catch (DbUpdateException)
        {
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DatabaseError);
        }
    }
}
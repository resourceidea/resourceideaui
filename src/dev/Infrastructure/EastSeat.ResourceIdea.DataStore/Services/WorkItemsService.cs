// ----------------------------------------------------------------------------------
// File: WorkItemsService.cs
// Path: src\dev\Infrastructure\EastSeat.ResourceIdea.DataStore\Services\WorkItemsService.cs
// Description: Service for managing work items.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Contracts;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;

using Microsoft.EntityFrameworkCore;

namespace EastSeat.ResourceIdea.DataStore.Services;

/// <summary>
/// Service for managing work items.
/// </summary>
public sealed class WorkItemsService(ResourceIdeaDBContext dbContext) : IWorkItemsService
{
    private readonly ResourceIdeaDBContext _dbContext = dbContext;

    /// <summary>
    /// Adds a new work item asynchronously.
    /// </summary>
    /// <param name="entity">The work item entity to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{WorkItem}"/>.</returns>
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

            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.EmptyEntityOnCreateWorkItem);
        }
        catch (DbUpdateException)
        {
            // Log the database update exception here if logging is available
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreCommandFailure);
        }
    }

    /// <summary>
    /// Deletes a work item asynchronously.
    /// </summary>
    /// <param name="entity">The work item entity to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{WorkItem}"/>.</returns>
    public async Task<ResourceIdeaResponse<WorkItem>> DeleteAsync(WorkItem entity, CancellationToken cancellationToken)
    {
        try
        {
            _dbContext.WorkItems.Remove(entity);
            int result = await _dbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return ResourceIdeaResponse<WorkItem>.Success(entity);
            }

            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreCommandFailure);
        }
        catch (DbUpdateException)
        {
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreCommandFailure);
        }
    }

    /// <summary>
    /// Gets a work item by ID asynchronously using a specification.
    /// </summary>
    /// <param name="specification">The specification to filter the work item.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{WorkItem}"/>.</returns>
    public async Task<ResourceIdeaResponse<WorkItem>> GetByIdAsync(BaseSpecification<WorkItem> specification, CancellationToken cancellationToken)
    {
        try
        {
            var workItem = await _dbContext.WorkItems.AsNoTracking().FirstOrDefaultAsync(specification.Criteria, cancellationToken);
            if (workItem != null)
            {
                return ResourceIdeaResponse<WorkItem>.Success(workItem);
            }

            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.NotFound);
        }
        catch (OperationCanceledException)
        {
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreQueryFailure);
        }
    }

    /// <summary>
    /// Gets a paged list of work items asynchronously.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="size">The number of items per page.</param>
    /// <param name="specification">The optional specification to filter the work items.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{PagedListResponse{WorkItem}}"/>.</returns>
    public async Task<ResourceIdeaResponse<PagedListResponse<WorkItem>>> GetPagedListAsync(int page, int size, Optional<BaseSpecification<WorkItem>> specification, CancellationToken cancellationToken)
    {
        try
        {
            var query = _dbContext.WorkItems.AsQueryable();
            
            if (specification.HasValue)
            {
                query = query.Where(specification.Value.Criteria);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            var pagedResponse = new PagedListResponse<WorkItem>
            {
                Items = items,
                CurrentPage = page,
                PageSize = size,
                TotalCount = totalCount
            };

            return ResourceIdeaResponse<PagedListResponse<WorkItem>>.Success(pagedResponse);
        }
        catch (OperationCanceledException)
        {
            return ResourceIdeaResponse<PagedListResponse<WorkItem>>.Failure(ErrorCode.DataStoreQueryFailure);
        }
    }

    /// <summary>
    /// Updates a work item asynchronously.
    /// </summary>
    /// <param name="entity">The work item entity to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{WorkItem}"/>.</returns>
    public async Task<ResourceIdeaResponse<WorkItem>> UpdateAsync(WorkItem entity, CancellationToken cancellationToken)
    {
        try
        {
            _dbContext.WorkItems.Update(entity);
            int result = await _dbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return ResourceIdeaResponse<WorkItem>.Success(entity);
            }

            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreCommandFailure);
        }
        catch (DbUpdateException)
        {
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreCommandFailure);
        }
    }
}
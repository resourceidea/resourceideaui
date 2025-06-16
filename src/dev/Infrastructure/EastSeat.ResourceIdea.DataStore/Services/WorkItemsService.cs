using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Contracts;
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

            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreInsertFailure);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error adding work item: {ex}");
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreInsertFailure);
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

            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreDeleteFailure);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error deleting work item: {ex}");
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreDeleteFailure);
        }
    }

    /// <summary>
    /// Gets a work item by its identifier asynchronously.
    /// </summary>
    /// <param name="specification">The specification to filter the work item.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{WorkItem}"/>.</returns>
    public async Task<ResourceIdeaResponse<WorkItem>> GetByIdAsync(
        BaseSpecification<WorkItem> specification, 
        CancellationToken cancellationToken)
    {
        try
        {
            var query = _dbContext.WorkItems
                .Include(w => w.Engagement)
                .Include(w => w.AssignedTo)
                .AsQueryable();

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            var workItem = await query.FirstOrDefaultAsync(cancellationToken);

            if (workItem == null)
            {
                return ResourceIdeaResponse<WorkItem>.NotFound();
            }

            return ResourceIdeaResponse<WorkItem>.Success(workItem);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error getting work item by id: {ex}");
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
    public async Task<ResourceIdeaResponse<PagedListResponse<WorkItem>>> GetPagedListAsync(
        int page, 
        int size, 
        Optional<BaseSpecification<WorkItem>> specification, 
        CancellationToken cancellationToken)
    {
        try
        {
            var query = _dbContext.WorkItems
                .Include(w => w.Engagement)
                .Include(w => w.AssignedTo)
                .AsQueryable();

            if (specification.HasValue && specification.Value.Criteria != null)
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
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error getting paged work items: {ex}");
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
                // Reload the entity to get updated navigation properties
                var updatedEntity = await _dbContext.WorkItems
                    .Include(w => w.Engagement)
                    .Include(w => w.AssignedTo)
                    .FirstOrDefaultAsync(w => w.Id == entity.Id, cancellationToken);

                return updatedEntity != null 
                    ? ResourceIdeaResponse<WorkItem>.Success(updatedEntity)
                    : ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.NotFound);
            }

            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreUpdateFailure);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error updating work item: {ex}");
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreUpdateFailure);
        }
    }
}
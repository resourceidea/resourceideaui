using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Contracts;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.Types;
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
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{WorkItem}"/>.</returns>
    public async Task<ResourceIdeaResponse<WorkItem>> AddAsync(WorkItem entity, CancellationToken cancellationToken)
    {
        try
        {
            _dbContext.WorkItems.Add(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ResourceIdeaResponse<WorkItem>.Success(entity);
        }
        catch (DbUpdateException dbEx)
        {
            Console.Error.WriteLine($"Database update error: {dbEx.Message}");
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreCommandFailure);
        }
        catch (OperationCanceledException ocEx)
        {
            Console.Error.WriteLine($"Operation canceled: {ocEx.Message}");
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreQueryFailure);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreQueryFailure);
        }
    }

    /// <summary>
    /// Deletes a work item asynchronously.
    /// </summary>
    /// <param name="entity">The work item entity to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{WorkItem}"/>.</returns>
    public async Task<ResourceIdeaResponse<WorkItem>> DeleteAsync(WorkItem entity, CancellationToken cancellationToken)
    {
        try
        {
            _dbContext.WorkItems.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ResourceIdeaResponse<WorkItem>.Success(entity);
        }
        catch (DbUpdateException dbEx)
        {
            Console.Error.WriteLine($"Database update error: {dbEx.Message}");
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreCommandFailure);
        }
        catch (OperationCanceledException ocEx)
        {
            Console.Error.WriteLine($"Operation canceled: {ocEx.Message}");
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreQueryFailure);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreQueryFailure);
        }
    }

    /// <summary>
    /// Retrieves a work item by ID with engagement and client information.
    /// </summary>
    /// <param name="specification">The specification to filter the work item.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{WorkItem}"/>.</returns>
    public async Task<ResourceIdeaResponse<WorkItem>> GetByIdAsync(BaseSpecification<WorkItem> specification, CancellationToken cancellationToken)
    {
        try
        {
            WorkItem? workItem = await _dbContext.WorkItems
                .Include(w => w.Engagement)
                    .ThenInclude(e => e!.Client)
                .Include(w => w.AssignedTo)
                .Where(specification.Criteria)
                .FirstOrDefaultAsync(cancellationToken);

            if (workItem == null)
            {
                return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.NotFound);
            }

            return ResourceIdeaResponse<WorkItem>.Success(workItem);
        }
        catch (DbUpdateException dbEx)
        {
            Console.Error.WriteLine($"Database update error: {dbEx.Message}");
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreCommandFailure);
        }
        catch (OperationCanceledException ocEx)
        {
            Console.Error.WriteLine($"Operation canceled: {ocEx.Message}");
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreQueryFailure);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreQueryFailure);
        }
    }

    /// <summary>
    /// Retrieves a paged list of work items asynchronously.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="size">The number of items per page.</param>
    /// <param name="specification">The optional specification to filter the work items.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{PagedListResponse{WorkItem}}"/>.</returns>
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
                    .ThenInclude(e => e!.Client)
                .Include(w => w.AssignedTo)
                .AsQueryable();

            if (specification.HasValue)
            {
                query = query.Where(specification.Value.Criteria);
            }

            var totalRecords = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalRecords / size);

            var workItems = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            var pagedList = new PagedListResponse<WorkItem>(
                workItems,
                page,
                size,
                totalRecords,
                totalPages,
                page > 1,
                page < totalPages);

            return ResourceIdeaResponse<PagedListResponse<WorkItem>>.Success(Optional<PagedListResponse<WorkItem>>.Some(pagedList));
        }
        catch (DbUpdateException dbEx)
        {
            Console.Error.WriteLine($"Database update error: {dbEx.Message}");
            return ResourceIdeaResponse<PagedListResponse<WorkItem>>.Failure(ErrorCode.DataStoreCommandFailure);
        }
        catch (OperationCanceledException ocEx)
        {
            Console.Error.WriteLine($"Operation canceled: {ocEx.Message}");
            return ResourceIdeaResponse<PagedListResponse<WorkItem>>.Failure(ErrorCode.DataStoreQueryFailure);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return ResourceIdeaResponse<PagedListResponse<WorkItem>>.Failure(ErrorCode.DataStoreQueryFailure);
        }
    }

    /// <summary>
    /// Updates a work item asynchronously.
    /// </summary>
    /// <param name="entity">The work item entity to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{WorkItem}"/>.</returns>
    public async Task<ResourceIdeaResponse<WorkItem>> UpdateAsync(WorkItem entity, CancellationToken cancellationToken)
    {
        try
        {
            _dbContext.WorkItems.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ResourceIdeaResponse<WorkItem>.Success(entity);
        }
        catch (DbUpdateException dbEx)
        {
            Console.Error.WriteLine($"Database update error: {dbEx.Message}");
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreCommandFailure);
        }
        catch (OperationCanceledException ocEx)
        {
            Console.Error.WriteLine($"Operation canceled: {ocEx.Message}");
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreQueryFailure);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreQueryFailure);
        }
    }
}
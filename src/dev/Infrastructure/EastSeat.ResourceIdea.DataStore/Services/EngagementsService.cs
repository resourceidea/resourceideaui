using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;

using Microsoft.EntityFrameworkCore;

namespace EastSeat.ResourceIdea.DataStore.Services;

/// <summary>
/// Service for managing engagements.
/// </summary>
public sealed class EngagementsService(ResourceIdeaDBContext dbContext) : IEngagementsService
{
    private readonly ResourceIdeaDBContext _dbContext = dbContext;
    /// <summary>
    /// Adds a new engagement asynchronously.
    /// </summary>
    /// <param name="entity">The engagement entity to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{Engagement}"/>.</returns>
    public Task<ResourceIdeaResponse<Engagement>> AddAsync(Engagement entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Cancels an engagement asynchronously.
    /// </summary>
    /// <param name="engagementId">The ID of the engagement to cancel.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{Engagement}"/>.</returns>
    public Task<ResourceIdeaResponse<Engagement>> CancelAsync(EngagementId engagementId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Completes an engagement asynchronously.
    /// </summary>
    /// <param name="engagementId">The ID of the engagement to complete.</param>
    /// <param name="completionDate">The completion date of the engagement.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{Engagement}"/>.</returns>
    public Task<ResourceIdeaResponse<Engagement>> CompleteAsync(EngagementId engagementId, DateTimeOffset completionDate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Deletes an engagement asynchronously.
    /// </summary>
    /// <param name="entity">The engagement entity to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{Engagement}"/>.</returns>
    public Task<ResourceIdeaResponse<Engagement>> DeleteAsync(Engagement entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets an engagement by ID asynchronously.
    /// </summary>
    /// <param name="specification">The specification to filter the engagement.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{Engagement}"/>.</returns>
    public async Task<ResourceIdeaResponse<Engagement>> GetByIdAsync(BaseSpecification<Engagement> specification, CancellationToken cancellationToken)
    {
        try
        {
            Engagement? engagement = await _dbContext.Engagements.AsNoTracking().FirstOrDefaultAsync(specification.Criteria, cancellationToken);
            if (engagement == null)
            {
                return ResourceIdeaResponse<Engagement>.NotFound();
            }

            return ResourceIdeaResponse<Engagement>.Success(engagement);
        }
        catch (OperationCanceledException)
        {
            return ResourceIdeaResponse<Engagement>.Failure(ErrorCode.DataStoreQueryFailure);
        }
        catch (InvalidOperationException ex)
        {
            // Log the invalid operation exception here if logging is available
            Console.Error.WriteLine($"Invalid operation: {ex.Message}");
            return ResourceIdeaResponse<Engagement>.Failure(ErrorCode.DataStoreQueryFailure);
        }
        catch (TimeoutException ex)
        {
            // Log the timeout exception here if logging is available
            Console.Error.WriteLine($"Operation timed out: {ex.Message}");
            return ResourceIdeaResponse<Engagement>.Failure(ErrorCode.DataStoreQueryFailure);
        }
        catch (Exception ex)
        {
            // Log the unexpected exception here if logging is available
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            throw; // Rethrow the exception to preserve its context
        }
    }

    /// <summary>
    /// Gets a paged list of engagements asynchronously.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="size">The number of items per page.</param>
    /// <param name="specification">The optional specification to filter the engagements.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{PagedListResponse{Engagement}}"/>.</returns>
    public async Task<ResourceIdeaResponse<PagedListResponse<Engagement>>> GetPagedListAsync(int page, int size, Optional<BaseSpecification<Engagement>> specification, CancellationToken cancellationToken)
    {
        try
        {
            IQueryable<Engagement> query = _dbContext.Engagements.AsQueryable();

            if (specification.HasValue)
            {
                query = query.Where(specification.Value.Criteria);
            }

            int totalCount = await query.CountAsync(cancellationToken);
            List<Engagement> items = await query
                                          .Skip((page - 1) * size)
                                          .Take(size)
                                          .OrderBy(e => e.Description)
                                          .ToListAsync(cancellationToken);

            PagedListResponse<Engagement> pagedList = new()
            {
                Items = items,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = size
            };

            return ResourceIdeaResponse<PagedListResponse<Engagement>>.Success(Optional<PagedListResponse<Engagement>>.Some(pagedList));
        }
        catch (DbUpdateException dbEx)
        {
            // Log the database update exception here if logging is available
            Console.Error.WriteLine($"Database update error: {dbEx.Message}");
            return ResourceIdeaResponse<PagedListResponse<Engagement>>.Failure(ErrorCode.DataStoreCommandFailure);
        }
        catch (OperationCanceledException ocEx)
        {
            // Log the operation canceled exception here if logging is available
            Console.Error.WriteLine($"Operation canceled: {ocEx.Message}");
            return ResourceIdeaResponse<PagedListResponse<Engagement>>.Failure(ErrorCode.DataStoreQueryFailure);
        }
        catch (Exception ex)
        {
            // Log the generic exception here if logging is available
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return ResourceIdeaResponse<PagedListResponse<Engagement>>.Failure(ErrorCode.DataStoreCommandFailure);
        }
    }

    /// <summary>
    /// Starts an engagement asynchronously.
    /// </summary>
    /// <param name="engagementId">The ID of the engagement to start.</param>
    /// <param name="commencementDate">The commencement date of the engagement.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{Engagement}"/>.</returns>
    public Task<ResourceIdeaResponse<Engagement>> StartAsync(EngagementId engagementId, DateTimeOffset commencementDate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Updates an engagement asynchronously.
    /// </summary>
    /// <param name="entity">The engagement entity to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation that returns a <see cref="ResourceIdeaResponse{Engagement}"/>.</returns>
    public async Task<ResourceIdeaResponse<Engagement>> UpdateAsync(Engagement entity, CancellationToken cancellationToken)
    {
        try
        {
            var existingEngagement = await _dbContext.Engagements.FirstOrDefaultAsync(e => e.Id == entity.Id, cancellationToken);
            if (existingEngagement == null)
            {
                return ResourceIdeaResponse<Engagement>.NotFound();
            }

            // Update properties (engagement ID and client ID are read-only as per requirements)
            existingEngagement.Description = entity.Description;
            existingEngagement.EngagementStatus = entity.EngagementStatus;
            
            // Handle nullable dates - only update if not MinValue (which represents null in our UI)
            existingEngagement.CommencementDate = entity.CommencementDate == DateTimeOffset.MinValue 
                ? null 
                : entity.CommencementDate;
            existingEngagement.CompletionDate = entity.CompletionDate == DateTimeOffset.MinValue 
                ? null 
                : entity.CompletionDate;

            _dbContext.Engagements.Update(existingEngagement);
            int result = await _dbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return ResourceIdeaResponse<Engagement>.Success(existingEngagement);
            }
            return ResourceIdeaResponse<Engagement>.Failure(ErrorCode.EmptyEntityOnUpdateEngagement);
        }
        catch (DbUpdateException)
        {
            return ResourceIdeaResponse<Engagement>.Failure(ErrorCode.DataStoreCommandFailure);
        }
        catch (OperationCanceledException)
        {
            return ResourceIdeaResponse<Engagement>.Failure(ErrorCode.DataStoreCommandFailure);
        }
        catch (Exception)
        {
            return ResourceIdeaResponse<Engagement>.Failure(ErrorCode.DataStoreCommandFailure);
        }
    }
}

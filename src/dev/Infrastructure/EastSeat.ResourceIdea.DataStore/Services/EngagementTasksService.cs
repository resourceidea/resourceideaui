using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.EngagementTasks.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.Entities;
using EastSeat.ResourceIdea.Domain.EngagementTasks.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

namespace EastSeat.ResourceIdea.DataStore.Services;

/// <summary>
/// Represents the service for managing engagement tasks.
/// </summary>
public sealed class EngagementTasksService : IEngagementTasksService
{
    /// <inheritdoc />
    public Task<ResourceIdeaResponse<EngagementTask>> AddAsync(EngagementTask entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<IReadOnlyList<EngagementTaskAssignment>>> AssignAsync(EngagementTaskId engagementTaskId, IReadOnlyList<ApplicationUserId> applicationUserIds, DateTimeOffset startDate, DateTimeOffset endDate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<EngagementTask>> BlockAsync(EngagementTaskId engagementTaskId, string? reason, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<EngagementTask>> CloseAsync(EngagementTaskId engagementTaskId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<EngagementTask>> CompleteAsync(EngagementTaskId engagementTaskId, DateTimeOffset completionDate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<EngagementTask>> DeleteAsync(EngagementTask entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<EngagementTask>> GetByIdAsync(BaseSpecification<EngagementTask> specification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<PagedListResponse<EngagementTask>>> GetPagedListAsync(int page, int size, Optional<BaseSpecification<EngagementTask>> specification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<EngagementTask>> RemoveAsync(EngagementTaskId engagementTaskId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<EngagementTask>> ReopenAsync(EngagementTaskId engagementTaskId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<EngagementTask>> SetAssignmentStatusFlagAsync(EngagementTaskId engagementTaskId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<Engagement>> StartAsync(EngagementTaskId engagementTaskId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<IReadOnlyList<EngagementTaskAssignment>>> UnassignAsync(EngagementTaskId engagementTaskId, IReadOnlyList<ApplicationUserId> applicationUserIds, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<EngagementTask>> UpdateAsync(EngagementTask entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

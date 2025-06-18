using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.DataStore;
using EastSeat.ResourceIdea.DataStore.Services;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace EastSeat.ResourceIdea.DataStore.UnitTests.Services;

/// <summary>
/// Unit tests for <see cref="WorkItemsService"/>.
/// </summary>
public class WorkItemsServiceTests : IDisposable
{
    private readonly ResourceIdeaDBContext _context;
    private readonly WorkItemsService _service;

    public WorkItemsServiceTests()
    {
        var options = new DbContextOptionsBuilder<ResourceIdeaDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ResourceIdeaDBContext(options);
        _service = new WorkItemsService(_context);
    }

    [Fact]
    public async Task AddAsync_WhenWorkItemIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var workItem = CreateTestWorkItem();

        // Act
        var result = await _service.AddAsync(workItem, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);
        Assert.Equal(workItem.Id, result.Content.Value.Id);
        Assert.Equal(workItem.Title, result.Content.Value.Title);
        Assert.Equal(workItem.Priority, result.Content.Value.Priority);

        // Verify it was saved to database
        var savedWorkItem = await _context.WorkItems.FindAsync(workItem.Id);
        Assert.NotNull(savedWorkItem);
        Assert.Equal(workItem.Title, savedWorkItem.Title);
    }

    [Fact]
    public async Task AddAsync_WhenDatabaseUpdateFails_ShouldReturnFailureResponse()
    {
        // Arrange
        var workItem = CreateTestWorkItem();
        
        // Dispose the context to simulate a database error
        await _context.DisposeAsync();

        // Act
        var result = await _service.AddAsync(workItem, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCode.DataStoreCommandFailure, result.Error);
    }

    [Fact]
    public async Task DeleteAsync_WhenWorkItemExists_ShouldReturnSuccessResponse()
    {
        // Arrange
        var workItem = CreateTestWorkItem();
        await _context.WorkItems.AddAsync(workItem);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.DeleteAsync(workItem, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);
        Assert.Equal(workItem.Id, result.Content.Value.Id);

        // Verify it was removed from database
        var deletedWorkItem = await _context.WorkItems.FindAsync(workItem.Id);
        Assert.Null(deletedWorkItem);
    }

    [Fact]
    public async Task DeleteAsync_WhenDatabaseUpdateFails_ShouldReturnFailureResponse()
    {
        // Arrange
        var workItem = CreateTestWorkItem();
        
        // Dispose the context to simulate a database error
        await _context.DisposeAsync();

        // Act
        var result = await _service.DeleteAsync(workItem, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCode.DataStoreCommandFailure, result.Error);
    }

    [Fact]
    public async Task GetByIdAsync_WhenWorkItemExists_ShouldReturnSuccessResponse()
    {
        // Arrange
        var workItem = CreateTestWorkItem();
        await _context.WorkItems.AddAsync(workItem);
        await _context.SaveChangesAsync();

        var specification = new TestWorkItemSpecification(w => w.Id == workItem.Id);

        // Act
        var result = await _service.GetByIdAsync(specification, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);
        Assert.Equal(workItem.Id, result.Content.Value.Id);
        Assert.Equal(workItem.Title, result.Content.Value.Title);
    }

    [Fact]
    public async Task GetByIdAsync_WhenWorkItemDoesNotExist_ShouldReturnNotFoundError()
    {
        // Arrange
        var nonExistentId = WorkItemId.NewId();
        var specification = new TestWorkItemSpecification(w => w.Id == nonExistentId);

        // Act
        var result = await _service.GetByIdAsync(specification, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCode.NotFound, result.Error);
    }

    [Fact]
    public async Task GetByIdAsync_WhenOperationIsCanceled_ShouldReturnFailureResponse()
    {
        // Arrange
        var workItem = CreateTestWorkItem();
        await _context.WorkItems.AddAsync(workItem);
        await _context.SaveChangesAsync();

        var specification = new TestWorkItemSpecification(w => w.Id == workItem.Id);
        var cancelledToken = new CancellationToken(true);

        // Act
        var result = await _service.GetByIdAsync(specification, cancelledToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCode.DataStoreQueryFailure, result.Error);
    }

    [Fact]
    public async Task GetPagedListAsync_WhenWorkItemsExist_ShouldReturnPagedResponse()
    {
        // Arrange
        var workItems = new[]
        {
            CreateTestWorkItem("Work Item 1"),
            CreateTestWorkItem("Work Item 2"),
            CreateTestWorkItem("Work Item 3")
        };

        await _context.WorkItems.AddRangeAsync(workItems);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetPagedListAsync(1, 2, Optional<BaseSpecification<WorkItem>>.None, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);
        
        var pagedResponse = result.Content.Value;
        Assert.Equal(1, pagedResponse.CurrentPage);
        Assert.Equal(2, pagedResponse.PageSize);
        Assert.Equal(3, pagedResponse.TotalCount);
        Assert.Equal(2, pagedResponse.Items.Count);
    }

    [Fact]
    public async Task GetPagedListAsync_WithSpecification_ShouldReturnFilteredResults()
    {
        // Arrange
        var tenantId = TenantId.Create(Guid.NewGuid());
        var workItems = new[]
        {
            CreateTestWorkItem("Work Item 1", tenantId),
            CreateTestWorkItem("Work Item 2", tenantId),
            CreateTestWorkItem("Work Item 3", TenantId.Create(Guid.NewGuid())) // Different tenant
        };

        await _context.WorkItems.AddRangeAsync(workItems);
        await _context.SaveChangesAsync();

        var specification = Optional<BaseSpecification<WorkItem>>.Some(
            new TestWorkItemSpecification(w => w.TenantId == tenantId));

        // Act
        var result = await _service.GetPagedListAsync(1, 10, specification, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);
        
        var pagedResponse = result.Content.Value;
        Assert.Equal(2, pagedResponse.TotalCount);
        Assert.Equal(2, pagedResponse.Items.Count);
        Assert.All(pagedResponse.Items, item => Assert.Equal(tenantId, item.TenantId));
    }

    [Fact]
    public async Task UpdateAsync_WhenWorkItemIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var workItem = CreateTestWorkItem();
        await _context.WorkItems.AddAsync(workItem);
        await _context.SaveChangesAsync();

        // Modify the work item
        workItem.Title = "Updated Title";
        workItem.Priority = Priority.High;

        // Act
        var result = await _service.UpdateAsync(workItem, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);
        Assert.Equal("Updated Title", result.Content.Value.Title);
        Assert.Equal(Priority.High, result.Content.Value.Priority);

        // Verify it was updated in database
        var updatedWorkItem = await _context.WorkItems.FindAsync(workItem.Id);
        Assert.NotNull(updatedWorkItem);
        Assert.Equal("Updated Title", updatedWorkItem.Title);
        Assert.Equal(Priority.High, updatedWorkItem.Priority);
    }

    [Fact]
    public async Task UpdateAsync_WhenDatabaseUpdateFails_ShouldReturnFailureResponse()
    {
        // Arrange
        var workItem = CreateTestWorkItem();
        
        // Dispose the context to simulate a database error
        await _context.DisposeAsync();

        // Act
        var result = await _service.UpdateAsync(workItem, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCode.DataStoreCommandFailure, result.Error);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    private static WorkItem CreateTestWorkItem(string title = "Test Work Item", TenantId? tenantId = null)
    {
        return new WorkItem
        {
            Id = WorkItemId.NewId(),
            Title = title,
            Description = "Test description",
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = tenantId ?? TenantId.Create(Guid.NewGuid()),
            StartDate = DateTimeOffset.UtcNow.AddDays(1),
            Status = WorkItemStatus.NotStarted,
            Priority = Priority.Medium
        };
    }

    private class TestWorkItemSpecification : BaseSpecification<WorkItem>
    {
        private readonly System.Linq.Expressions.Expression<Func<WorkItem, bool>> _criteria;

        public TestWorkItemSpecification(System.Linq.Expressions.Expression<Func<WorkItem, bool>> criteria)
        {
            _criteria = criteria;
        }

        public override System.Linq.Expressions.Expression<Func<WorkItem, bool>> Criteria => _criteria;
    }
}
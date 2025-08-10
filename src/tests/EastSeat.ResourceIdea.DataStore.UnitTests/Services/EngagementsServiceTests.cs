using System.Linq.Expressions;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.DataStore.Services;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EastSeat.ResourceIdea.DataStore.UnitTests.Services;

/// <summary>
/// Unit tests for the EngagementsService class.
/// </summary>
public class EngagementsServiceTests : IDisposable
{
    private readonly ResourceIdeaDBContext _dbContext;
    private readonly EngagementsService _engagementsService;
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    public EngagementsServiceTests()
    {
        // Setup in-memory database for testing
        var options = new DbContextOptionsBuilder<ResourceIdeaDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ResourceIdeaDBContext(options);
        _engagementsService = new EngagementsService(_dbContext);
    }

    /// <summary>
    /// Test specification for finding engagement by ID.
    /// </summary>
    private class EngagementByIdSpecification : BaseSpecification<Engagement>
    {
        private readonly EngagementId _engagementId;

        public EngagementByIdSpecification(EngagementId engagementId)
        {
            _engagementId = engagementId;
        }

        public override Expression<Func<Engagement, bool>> Criteria => e => e.Id == _engagementId;
    }

    [Fact]
    public async Task GetByIdAsync_WithValidSpecification_ReturnsSuccessResponse()
    {
        // Arrange
        var tenantId = TenantId.Create(Guid.NewGuid());
        var clientId = ClientId.Create(Guid.NewGuid());
        var engagementId = EngagementId.Create(Guid.NewGuid());

        var client = new Client
        {
            Id = clientId,
            TenantId = tenantId,
            Name = "Test Client",
            CreatedBy = Guid.NewGuid().ToString(),
            Created = DateTimeOffset.UtcNow,
            LastModifiedBy = Guid.NewGuid().ToString(),
            LastModified = DateTimeOffset.UtcNow
        };

        var engagement = new Engagement
        {
            Id = engagementId,
            ClientId = clientId,
            TenantId = tenantId,
            Description = "Test Engagement",
            EngagementStatus = EngagementStatus.InProgress,
            Client = client,
            CreatedBy = Guid.NewGuid().ToString(),
            Created = DateTimeOffset.UtcNow,
            LastModifiedBy = Guid.NewGuid().ToString(),
            LastModified = DateTimeOffset.UtcNow
        };

        await _dbContext.Clients.AddAsync(client, _cancellationToken);
        await _dbContext.Engagements.AddAsync(engagement, _cancellationToken);
        await _dbContext.SaveChangesAsync(_cancellationToken);

        var specification = new EngagementByIdSpecification(engagementId);

        // Act
        var result = await _engagementsService.GetByIdAsync(specification, _cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);
        var data = result.Content.Value;
        Assert.Equal(engagementId, data.Id);
        Assert.Equal("Test Engagement", data.Description);
        Assert.Equal(EngagementStatus.InProgress, data.EngagementStatus);
        Assert.NotNull(data.Client);
        Assert.Equal("Test Client", data.Client.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistentEngagement_ReturnsFailureResponse()
    {
        // Arrange
        var nonExistentEngagementId = EngagementId.Create(Guid.NewGuid());
        var specification = new EngagementByIdSpecification(nonExistentEngagementId);

        // Act
        var result = await _engagementsService.GetByIdAsync(specification, _cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.DataStoreQueryFailure, result.Error);
        Assert.False(result.Content.HasValue);
    }

    [Fact]
    public async Task GetByIdAsync_WithCancelledToken_ReturnsFailureResponse()
    {
        // Arrange
        var engagementId = EngagementId.Create(Guid.NewGuid());
        var specification = new EngagementByIdSpecification(engagementId);

        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        // Act
        var result = await _engagementsService.GetByIdAsync(specification, cancellationTokenSource.Token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.DataStoreQueryFailure, result.Error);
        Assert.False(result.Content.HasValue);
    }

    [Fact]
    public async Task GetByIdAsync_WithComplexSpecification_ReturnsCorrectEngagement()
    {
        // Arrange
        var tenantId = TenantId.Create(Guid.NewGuid());
        var clientId = ClientId.Create(Guid.NewGuid());
        var engagementId1 = EngagementId.Create(Guid.NewGuid());
        var engagementId2 = EngagementId.Create(Guid.NewGuid());

        var client = new Client
        {
            Id = clientId,
            TenantId = tenantId,
            Name = "Test Client",
            CreatedBy = Guid.NewGuid().ToString(),
            Created = DateTimeOffset.UtcNow,
            LastModifiedBy = Guid.NewGuid().ToString(),
            LastModified = DateTimeOffset.UtcNow
        };

        var engagement1 = new Engagement
        {
            Id = engagementId1,
            ClientId = clientId,
            TenantId = tenantId,
            Description = "InProgress Engagement",
            EngagementStatus = EngagementStatus.InProgress,
            Client = client,
            CreatedBy = Guid.NewGuid().ToString(),
            Created = DateTimeOffset.UtcNow,
            LastModifiedBy = Guid.NewGuid().ToString(),
            LastModified = DateTimeOffset.UtcNow
        };

        var engagement2 = new Engagement
        {
            Id = engagementId2,
            ClientId = clientId,
            TenantId = tenantId,
            Description = "Completed Engagement",
            EngagementStatus = EngagementStatus.Completed,
            Client = client,
            CreatedBy = Guid.NewGuid().ToString(),
            Created = DateTimeOffset.UtcNow,
            LastModifiedBy = Guid.NewGuid().ToString(),
            LastModified = DateTimeOffset.UtcNow
        };

        await _dbContext.Clients.AddAsync(client, _cancellationToken);
        await _dbContext.Engagements.AddRangeAsync([engagement1, engagement2], _cancellationToken);
        await _dbContext.SaveChangesAsync(_cancellationToken);

        // Create specification to find InProgress engagement with specific ID
        var specification = new InProgressEngagementByIdSpecification(engagementId1);

        // Act
        var result = await _engagementsService.GetByIdAsync(specification, _cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);
        var data = result.Content.Value;
        Assert.Equal(engagementId1, data.Id);
        Assert.Equal("InProgress Engagement", data.Description);
        Assert.Equal(EngagementStatus.InProgress, data.EngagementStatus);
    }

    [Fact]
    public async Task GetByIdAsync_WithComplexSpecificationNoMatch_ReturnsFailureResponse()
    {
        // Arrange
        var tenantId = TenantId.Create(Guid.NewGuid());
        var clientId = ClientId.Create(Guid.NewGuid());
        var engagementId = EngagementId.Create(Guid.NewGuid());

        var client = new Client
        {
            Id = clientId,
            TenantId = tenantId,
            Name = "Test Client",
            CreatedBy = Guid.NewGuid().ToString(),
            Created = DateTimeOffset.UtcNow,
            LastModifiedBy = Guid.NewGuid().ToString(),
            LastModified = DateTimeOffset.UtcNow
        };

        var engagement = new Engagement
        {
            Id = engagementId,
            ClientId = clientId,
            TenantId = tenantId,
            Description = "Completed Engagement",
            EngagementStatus = EngagementStatus.Completed,
            Client = client,
            CreatedBy = Guid.NewGuid().ToString(),
            Created = DateTimeOffset.UtcNow,
            LastModifiedBy = Guid.NewGuid().ToString(),
            LastModified = DateTimeOffset.UtcNow
        };

        await _dbContext.Clients.AddAsync(client, _cancellationToken);
        await _dbContext.Engagements.AddAsync(engagement, _cancellationToken);
        await _dbContext.SaveChangesAsync(_cancellationToken);

        // Try to find an InProgress engagement, but the engagement is completed
        var specification = new InProgressEngagementByIdSpecification(engagementId);

        // Act
        var result = await _engagementsService.GetByIdAsync(specification, _cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.DataStoreQueryFailure, result.Error);
        Assert.False(result.Content.HasValue);
    }

    [Fact]
    public async Task GetByIdAsync_IncludesClientNavigationProperty()
    {
        // Arrange
        var tenantId = TenantId.Create(Guid.NewGuid());
        var clientId = ClientId.Create(Guid.NewGuid());
        var engagementId = EngagementId.Create(Guid.NewGuid());

        var client = new Client
        {
            Id = clientId,
            TenantId = tenantId,
            Name = "Test Client with Navigation",
            CreatedBy = Guid.NewGuid().ToString(),
            Created = DateTimeOffset.UtcNow,
            LastModifiedBy = Guid.NewGuid().ToString(),
            LastModified = DateTimeOffset.UtcNow
        };

        var engagement = new Engagement
        {
            Id = engagementId,
            ClientId = clientId,
            TenantId = tenantId,
            Description = "Test Engagement with Client",
            EngagementStatus = EngagementStatus.InProgress,
            CreatedBy = Guid.NewGuid().ToString(),
            Created = DateTimeOffset.UtcNow,
            LastModifiedBy = Guid.NewGuid().ToString(),
            LastModified = DateTimeOffset.UtcNow
        };

        await _dbContext.Clients.AddAsync(client, _cancellationToken);
        await _dbContext.Engagements.AddAsync(engagement, _cancellationToken);
        await _dbContext.SaveChangesAsync(_cancellationToken);

        var specification = new EngagementByIdSpecification(engagementId);

        // Act
        var result = await _engagementsService.GetByIdAsync(specification, _cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);
        var data = result.Content.Value;
        Assert.NotNull(data.Client);
        Assert.Equal("Test Client with Navigation", data.Client.Name);
        Assert.Equal(clientId, data.Client.Id);
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
    }

    /// <summary>
    /// Test specification for finding InProgress engagement by ID.
    /// </summary>
    private class InProgressEngagementByIdSpecification : BaseSpecification<Engagement>
    {
        private readonly EngagementId _engagementId;

        public InProgressEngagementByIdSpecification(EngagementId engagementId)
        {
            _engagementId = engagementId;
        }

        public override Expression<Func<Engagement, bool>> Criteria =>
            e => e.Id == _engagementId && e.EngagementStatus == EngagementStatus.InProgress;
    }
}

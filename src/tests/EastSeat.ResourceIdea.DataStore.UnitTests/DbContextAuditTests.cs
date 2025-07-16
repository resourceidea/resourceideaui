// -------------------------------------------------------------------------------
// File: DbContextAuditTests.cs
// Path: src\tests\EastSeat.ResourceIdea.DataStore.UnitTests\DbContextAuditTests.cs
// Description: Tests for DbContext audit field population.
// -------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EastSeat.ResourceIdea.DataStore.UnitTests;

public class DbContextAuditTests
{
    private class TestAuthenticationContext : IAuthenticationContext
    {
        public TenantId TenantId { get; set; }
        public ApplicationUserId ApplicationUserId { get; set; }
    }

    [Fact]
    public async Task SaveChangesAsync_Should_PopulateAuditFields_WhenAddingEntity()
    {
        // Arrange
        var tenantId = TenantId.Create(Guid.NewGuid());
        var applicationUserId = ApplicationUserId.Create(Guid.NewGuid());
        
        var authContext = new TestAuthenticationContext
        {
            TenantId = tenantId,
            ApplicationUserId = applicationUserId
        };

        var options = new DbContextOptionsBuilder<ResourceIdeaDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ResourceIdeaDBContext(options, authContext);

        var client = new Client
        {
            Id = ClientId.Create(Guid.NewGuid()),
            Name = "Test Client"
        };

        // Act
        context.Clients.Add(client);
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(tenantId, client.TenantId);
        Assert.Equal(applicationUserId.ToString(), client.CreatedBy);
        Assert.Equal(applicationUserId.ToString(), client.LastModifiedBy);
        Assert.True(client.Created > DateTimeOffset.MinValue);
        Assert.True(client.LastModified > DateTimeOffset.MinValue);
        Assert.False(client.IsDeleted);
    }

    [Fact]
    public async Task SaveChangesAsync_Should_UpdateModifiedFields_WhenModifyingEntity()
    {
        // Arrange
        var tenantId = TenantId.Create(Guid.NewGuid());
        var applicationUserId = ApplicationUserId.Create(Guid.NewGuid());
        
        var authContext = new TestAuthenticationContext
        {
            TenantId = tenantId,
            ApplicationUserId = applicationUserId
        };

        var options = new DbContextOptionsBuilder<ResourceIdeaDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ResourceIdeaDBContext(options, authContext);

        var client = new Client
        {
            Id = ClientId.Create(Guid.NewGuid()),
            Name = "Test Client"
        };

        context.Clients.Add(client);
        await context.SaveChangesAsync();

        var originalLastModified = client.LastModified;
        var originalCreatedBy = client.CreatedBy;

        // Act
        await Task.Delay(10); // Small delay to ensure different timestamp
        client.Name = "Updated Client";
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(originalCreatedBy, client.CreatedBy); // CreatedBy should not change
        Assert.Equal(applicationUserId.ToString(), client.LastModifiedBy);
        Assert.True(client.LastModified > originalLastModified);
    }

    [Fact]
    public async Task SaveChangesAsync_Should_HandleSoftDelete_WhenDeletingEntity()
    {
        // Arrange
        var tenantId = TenantId.Create(Guid.NewGuid());
        var applicationUserId = ApplicationUserId.Create(Guid.NewGuid());
        
        var authContext = new TestAuthenticationContext
        {
            TenantId = tenantId,
            ApplicationUserId = applicationUserId
        };

        var options = new DbContextOptionsBuilder<ResourceIdeaDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ResourceIdeaDBContext(options, authContext);

        var client = new Client
        {
            Id = ClientId.Create(Guid.NewGuid()),
            Name = "Test Client"
        };

        context.Clients.Add(client);
        await context.SaveChangesAsync();

        // Act
        context.Clients.Remove(client);
        await context.SaveChangesAsync();

        // Assert
        Assert.True(client.IsDeleted);
        Assert.Equal(applicationUserId.ToString(), client.DeletedBy);
        Assert.True(client.Deleted.HasValue);
        Assert.Equal(applicationUserId.ToString(), client.LastModifiedBy);
    }
}
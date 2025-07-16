// -------------------------------------------------------------------------------
// File: AuthenticationContextTests.cs
// Path: src\tests\EastSeat.ResourceIdea.Application.UnitTests\Features\Common\AuthenticationContextTests.cs
// Description: Tests for authentication context integration.
// -------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using Xunit;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.Common;

public class AuthenticationContextTests
{
    private class TestAuthenticationContext : IAuthenticationContext
    {
        public TenantId TenantId { get; set; }
        public ApplicationUserId ApplicationUserId { get; set; }
    }

    [Fact]
    public void IAuthenticationContext_Should_Have_Required_Properties()
    {
        // Arrange
        var tenantId = TenantId.Create(Guid.NewGuid());
        var applicationUserId = ApplicationUserId.Create(Guid.NewGuid());

        // Act
        var context = new TestAuthenticationContext
        {
            TenantId = tenantId,
            ApplicationUserId = applicationUserId
        };

        // Assert
        Assert.Equal(tenantId, context.TenantId);
        Assert.Equal(applicationUserId, context.ApplicationUserId);
    }

    [Fact]
    public void ApplicationUserId_Should_Have_ToString_Implementation()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var applicationUserId = ApplicationUserId.Create(guid);

        // Act
        var stringRepresentation = applicationUserId.ToString();

        // Assert
        Assert.Equal(guid.ToString(), stringRepresentation);
    }

    [Fact]
    public void TenantId_Should_Have_IsNotEmpty_Method()
    {
        // Arrange
        var tenantId = TenantId.Create(Guid.NewGuid());

        // Act
        var isNotEmpty = tenantId.IsNotEmpty();

        // Assert
        Assert.True(isNotEmpty);
    }
}
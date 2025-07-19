// -------------------------------------------------------------------------------
// File: BackendAccessTests.cs
// Path: src/tests/EastSeat.ResourceIdea.Web.UnitTests/Authorization/BackendAccessTests.cs
// Description: Unit tests for backend access authorization functionality
// -------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Web.Authorization;
using EastSeat.ResourceIdea.Web.RequestContext;
using Microsoft.AspNetCore.Authorization;
using Moq;
using System.Security.Claims;
using Xunit;

namespace EastSeat.ResourceIdea.Web.UnitTests.Authorization;

public class BackendAccessTests
{
    [Fact]
    public void BackendAccessAttribute_ShouldSetCorrectPolicy()
    {
        // Arrange & Act
        var attribute = new BackendAccessAttribute();

        // Assert
        Assert.Equal("BackendAccess", attribute.Policy);
    }

    [Fact]
    public void BackendAccessRequirement_ShouldHaveCorrectDescription()
    {
        // Arrange & Act
        var requirement = new BackendAccessRequirement();

        // Assert
        Assert.Equal("User must have backend access (Developer or Support role)", requirement.Description);
    }

    [Fact]
    public async Task BackendAccessHandler_ShouldSucceed_WhenUserHasBackendAccess()
    {
        // Arrange
        var mockRequestContext = new Mock<IResourceIdeaRequestContext>();
        mockRequestContext.Setup(x => x.HasBackendAccess()).Returns(true);

        var handler = new BackendAccessHandler(mockRequestContext.Object);
        var requirement = new BackendAccessRequirement();
        var user = new ClaimsPrincipal();
        var context = new AuthorizationHandlerContext(new[] { requirement }, user, null);

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.True(context.HasSucceeded);
        Assert.False(context.HasFailed);
    }

    [Fact]
    public async Task BackendAccessHandler_ShouldFail_WhenUserDoesNotHaveBackendAccess()
    {
        // Arrange
        var mockRequestContext = new Mock<IResourceIdeaRequestContext>();
        mockRequestContext.Setup(x => x.HasBackendAccess()).Returns(false);

        var handler = new BackendAccessHandler(mockRequestContext.Object);
        var requirement = new BackendAccessRequirement();
        var user = new ClaimsPrincipal();
        var context = new AuthorizationHandlerContext(new[] { requirement }, user, null);

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
        Assert.True(context.HasFailed);
    }

    [Fact]
    public async Task BackendAccessHandler_ShouldFail_WhenExceptionThrown()
    {
        // Arrange
        var mockRequestContext = new Mock<IResourceIdeaRequestContext>();
        mockRequestContext.Setup(x => x.HasBackendAccess()).Throws(new InvalidOperationException("Test exception"));

        var handler = new BackendAccessHandler(mockRequestContext.Object);
        var requirement = new BackendAccessRequirement();
        var user = new ClaimsPrincipal();
        var context = new AuthorizationHandlerContext(new[] { requirement }, user, null);

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
        Assert.True(context.HasFailed);
    }
}
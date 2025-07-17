using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using EastSeat.ResourceIdea.Web.RequestContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;
using Xunit;

namespace EastSeat.ResourceIdea.Web.UnitTests.Authentication;

public class LogoutTests
{
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<IUserStore<ApplicationUser>> _mockUserStore;

    public LogoutTests()
    {
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockUserStore = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            _mockUserStore.Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<ApplicationUser>>().Object,
            Array.Empty<IUserValidator<ApplicationUser>>(),
            Array.Empty<IPasswordValidator<ApplicationUser>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
    }

    [Fact]
    public void AuthenticationContext_Should_Return_Empty_When_User_Not_Authenticated()
    {
        // Arrange
        var claimsIdentity = new ClaimsIdentity(); // Not authenticated
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        var context = new ResourceIdeaRequestContext(_mockHttpContextAccessor.Object, _mockUserManager.Object);

        // Act
        var applicationUserId = ((IAuthenticationContext)context).ApplicationUserId;
        var tenantId = ((IAuthenticationContext)context).TenantId;

        // Assert
        Assert.Equal(ApplicationUserId.Empty, applicationUserId);
        Assert.True(tenantId.IsNotEmpty()); // Should return fallback TenantId
    }

    [Fact]
    public void AuthenticationContext_Should_Return_Empty_When_HttpContext_Is_Null()
    {
        // Arrange
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns((HttpContext?)null);

        var context = new ResourceIdeaRequestContext(_mockHttpContextAccessor.Object, _mockUserManager.Object);

        // Act
        var applicationUserId = ((IAuthenticationContext)context).ApplicationUserId;
        var tenantId = ((IAuthenticationContext)context).TenantId;

        // Assert
        Assert.Equal(ApplicationUserId.Empty, applicationUserId);
        Assert.True(tenantId.IsNotEmpty()); // Should return fallback TenantId
    }

    [Fact]
    public void AuthenticationContext_Should_Return_Empty_When_User_Is_Null()
    {
        // Arrange
        var httpContext = new DefaultHttpContext { User = null! };
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        var context = new ResourceIdeaRequestContext(_mockHttpContextAccessor.Object, _mockUserManager.Object);

        // Act
        var applicationUserId = ((IAuthenticationContext)context).ApplicationUserId;
        var tenantId = ((IAuthenticationContext)context).TenantId;

        // Assert
        Assert.Equal(ApplicationUserId.Empty, applicationUserId);
        Assert.True(tenantId.IsNotEmpty()); // Should return fallback TenantId
    }

    [Fact]
    public void AuthenticationContext_Should_Return_User_Data_When_Authenticated()
    {
        // Arrange
        var userName = "test@example.com";
        var applicationUser = new ApplicationUser
        {
            UserName = userName,
            ApplicationUserId = ApplicationUserId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        var claimsIdentity = new ClaimsIdentity("TestAuth");
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, userName));
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };

        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        _mockUserManager.Setup(x => x.FindByNameAsync(userName))
            .ReturnsAsync(applicationUser);

        var context = new ResourceIdeaRequestContext(_mockHttpContextAccessor.Object, _mockUserManager.Object);

        // Act
        var applicationUserId = ((IAuthenticationContext)context).ApplicationUserId;
        var tenantId = ((IAuthenticationContext)context).TenantId;

        // Assert
        Assert.Equal(applicationUser.ApplicationUserId, applicationUserId);
        Assert.Equal(applicationUser.TenantId, tenantId);
    }

    [Fact]
    public void AuthenticationContext_Should_Handle_User_Not_Found_In_Database()
    {
        // Arrange
        var userName = "test@example.com";
        var claimsIdentity = new ClaimsIdentity("TestAuth");
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, userName));
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };

        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        _mockUserManager.Setup(x => x.FindByNameAsync(userName))
            .ReturnsAsync((ApplicationUser?)null); // User not found

        var context = new ResourceIdeaRequestContext(_mockHttpContextAccessor.Object, _mockUserManager.Object);

        // Act
        var applicationUserId = ((IAuthenticationContext)context).ApplicationUserId;
        var tenantId = ((IAuthenticationContext)context).TenantId;

        // Assert
        Assert.Equal(ApplicationUserId.Empty, applicationUserId);
        Assert.True(tenantId.IsNotEmpty()); // Should return fallback TenantId
    }

    [Fact]
    public void AuthenticationContext_Should_Handle_Empty_UserName()
    {
        // Arrange
        var claimsIdentity = new ClaimsIdentity("TestAuth");
        // No name claim added
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };

        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        var context = new ResourceIdeaRequestContext(_mockHttpContextAccessor.Object, _mockUserManager.Object);

        // Act
        var applicationUserId = ((IAuthenticationContext)context).ApplicationUserId;
        var tenantId = ((IAuthenticationContext)context).TenantId;

        // Assert
        Assert.Equal(ApplicationUserId.Empty, applicationUserId);
        Assert.True(tenantId.IsNotEmpty()); // Should return fallback TenantId
    }

    [Fact]
    public void AuthenticationContext_Should_Handle_UserManager_Exception()
    {
        // Arrange
        var userName = "test@example.com";
        var claimsIdentity = new ClaimsIdentity("TestAuth");
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, userName));
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };

        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        _mockUserManager.Setup(x => x.FindByNameAsync(userName))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        var context = new ResourceIdeaRequestContext(_mockHttpContextAccessor.Object, _mockUserManager.Object);

        // Act
        var applicationUserId = ((IAuthenticationContext)context).ApplicationUserId;
        var tenantId = ((IAuthenticationContext)context).TenantId;

        // Assert
        Assert.Equal(ApplicationUserId.Empty, applicationUserId);
        Assert.True(tenantId.IsNotEmpty()); // Should return fallback TenantId
    }

    [Fact]
    public void AuthenticationContext_Should_Simulate_Logout_State_Transition()
    {
        // Arrange
        var userName = "test@example.com";
        var applicationUser = new ApplicationUser
        {
            UserName = userName,
            ApplicationUserId = ApplicationUserId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        // Initially authenticated
        var authenticatedIdentity = new ClaimsIdentity("TestAuth");
        authenticatedIdentity.AddClaim(new Claim(ClaimTypes.Name, userName));
        var authenticatedPrincipal = new ClaimsPrincipal(authenticatedIdentity);
        var httpContext = new DefaultHttpContext { User = authenticatedPrincipal };

        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        _mockUserManager.Setup(x => x.FindByNameAsync(userName))
            .ReturnsAsync(applicationUser);

        var context = new ResourceIdeaRequestContext(_mockHttpContextAccessor.Object, _mockUserManager.Object);

        // Act - Before logout (authenticated)
        var beforeLogoutUserId = ((IAuthenticationContext)context).ApplicationUserId;
        var beforeLogoutTenantId = ((IAuthenticationContext)context).TenantId;

        // Simulate logout by changing to unauthenticated state
        var unauthenticatedIdentity = new ClaimsIdentity(); // Not authenticated
        var unauthenticatedPrincipal = new ClaimsPrincipal(unauthenticatedIdentity);
        httpContext.User = unauthenticatedPrincipal;

        // Act - After logout (unauthenticated)
        var afterLogoutUserId = ((IAuthenticationContext)context).ApplicationUserId;
        var afterLogoutTenantId = ((IAuthenticationContext)context).TenantId;

        // Assert
        // Before logout should return user data
        Assert.Equal(applicationUser.ApplicationUserId, beforeLogoutUserId);
        Assert.Equal(applicationUser.TenantId, beforeLogoutTenantId);

        // After logout should return empty/fallback values
        Assert.Equal(ApplicationUserId.Empty, afterLogoutUserId);
        Assert.True(afterLogoutTenantId.IsNotEmpty()); // Should return fallback TenantId
        Assert.NotEqual(beforeLogoutTenantId, afterLogoutTenantId); // Should be different (fallback vs user's tenant)
    }
}
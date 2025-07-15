// =============================================================================================
// File: ApplicationUserServiceTests.cs
// Path: src/tests/EastSeat.ResourceIdea.DataStore.UnitTests/Services/ApplicationUserServiceTests.cs
// Description: Unit tests for ApplicationUserService to verify name synchronization with Employee table.
// =============================================================================================

using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.DataStore.Services;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace EastSeat.ResourceIdea.DataStore.UnitTests.Services;

public class ApplicationUserServiceTests : IDisposable
{
    private readonly ResourceIdeaDBContext _dbContext;
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly ApplicationUserService _service;

    public ApplicationUserServiceTests()
    {
        var options = new DbContextOptionsBuilder<ResourceIdeaDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _dbContext = new ResourceIdeaDBContext(options);
        
        _mockUserManager = MockUserManager();
        _service = new ApplicationUserService(_mockUserManager.Object, _dbContext);
    }

    [Fact]
    public async Task UpdateApplicationUserAsync_ValidUser_ShouldUpdateBothUserAndEmployee()
    {
        // Arrange
        var applicationUserId = ApplicationUserId.Create(Guid.NewGuid());
        var employeeId = EmployeeId.NewId();
        var tenantId = TenantId.Create(Guid.NewGuid());

        var applicationUser = new ApplicationUser
        {
            Id = applicationUserId.ToString(),
            ApplicationUserId = applicationUserId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            TenantId = tenantId
        };

        var employee = new Employee
        {
            EmployeeId = employeeId,
            ApplicationUserId = applicationUserId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            TenantId = tenantId
        };

        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.SaveChangesAsync();

        _mockUserManager
            .Setup(um => um.FindByIdAsync(applicationUserId.ToString()))
            .ReturnsAsync(applicationUser);

        _mockUserManager
            .Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service.UpdateApplicationUserAsync(applicationUserId, "Jane", "Smith");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);

        var updatedUser = result.Content.Value;
        Assert.Equal("Jane", updatedUser.FirstName);
        Assert.Equal("Smith", updatedUser.LastName);

        // Verify Employee was also updated
        var updatedEmployee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.ApplicationUserId == applicationUserId);
        Assert.NotNull(updatedEmployee);
        Assert.Equal("Jane", updatedEmployee.FirstName);
        Assert.Equal("Smith", updatedEmployee.LastName);

        _mockUserManager.Verify(um => um.FindByIdAsync(applicationUserId.ToString()), Times.Once);
        _mockUserManager.Verify(um => um.UpdateAsync(It.Is<ApplicationUser>(u => 
            u.FirstName == "Jane" && u.LastName == "Smith")), Times.Once);
    }

    [Fact]
    public async Task UpdateApplicationUserAsync_UserNotFound_ShouldReturnFailure()
    {
        // Arrange
        var applicationUserId = ApplicationUserId.Create(Guid.NewGuid());

        _mockUserManager
            .Setup(um => um.FindByIdAsync(applicationUserId.ToString()))
            .ReturnsAsync((ApplicationUser?)null);

        // Act
        var result = await _service.UpdateApplicationUserAsync(applicationUserId, "Jane", "Smith");

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCode.ApplicationUserNotFound, result.Error);

        _mockUserManager.Verify(um => um.FindByIdAsync(applicationUserId.ToString()), Times.Once);
        _mockUserManager.Verify(um => um.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
    }

    [Fact]
    public async Task UpdateApplicationUserAsync_UpdateUserFails_ShouldReturnFailure()
    {
        // Arrange
        var applicationUserId = ApplicationUserId.Create(Guid.NewGuid());

        var applicationUser = new ApplicationUser
        {
            Id = applicationUserId.ToString(),
            ApplicationUserId = applicationUserId,
            FirstName = "John",
            LastName = "Doe"
        };

        _mockUserManager
            .Setup(um => um.FindByIdAsync(applicationUserId.ToString()))
            .ReturnsAsync(applicationUser);

        _mockUserManager
            .Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Update failed" }));

        // Act
        var result = await _service.UpdateApplicationUserAsync(applicationUserId, "Jane", "Smith");

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCode.UpdateApplicationUserFailure, result.Error);

        _mockUserManager.Verify(um => um.FindByIdAsync(applicationUserId.ToString()), Times.Once);
        _mockUserManager.Verify(um => um.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);
    }

    [Fact]
    public async Task UpdateApplicationUserAsync_UserWithoutEmployee_ShouldUpdateOnlyUser()
    {
        // Arrange
        var applicationUserId = ApplicationUserId.Create(Guid.NewGuid());

        var applicationUser = new ApplicationUser
        {
            Id = applicationUserId.ToString(),
            ApplicationUserId = applicationUserId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        _mockUserManager
            .Setup(um => um.FindByIdAsync(applicationUserId.ToString()))
            .ReturnsAsync(applicationUser);

        _mockUserManager
            .Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service.UpdateApplicationUserAsync(applicationUserId, "Jane", "Smith");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);

        var updatedUser = result.Content.Value;
        Assert.Equal("Jane", updatedUser.FirstName);
        Assert.Equal("Smith", updatedUser.LastName);

        // Verify no Employee exists
        var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.ApplicationUserId == applicationUserId);
        Assert.Null(employee);
    }

    private static Mock<UserManager<ApplicationUser>> MockUserManager()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        var options = new Mock<IOptions<IdentityOptions>>();
        var passwordHasher = new Mock<IPasswordHasher<ApplicationUser>>();
        var userValidators = new List<IUserValidator<ApplicationUser>>();
        var passwordValidators = new List<IPasswordValidator<ApplicationUser>>();
        var keyNormalizer = new Mock<ILookupNormalizer>();
        var errors = new Mock<IdentityErrorDescriber>();
        var services = new Mock<IServiceProvider>();
        var logger = new Mock<ILogger<UserManager<ApplicationUser>>>();

        var mgr = new Mock<UserManager<ApplicationUser>>(store.Object, options.Object, passwordHasher.Object,
            userValidators, passwordValidators, keyNormalizer.Object, errors.Object, services.Object, logger.Object);
        
        return mgr;
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
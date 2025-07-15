// =============================================================================================
// File: EmployeesServiceSynchronizationTests.cs
// Path: src/tests/EastSeat.ResourceIdea.DataStore.UnitTests/Services/EmployeesServiceSynchronizationTests.cs
// Description: Integration tests for EmployeesService to verify ApplicationUser/Employee name synchronization.
// =============================================================================================

using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.DataStore.Services;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace EastSeat.ResourceIdea.DataStore.UnitTests.Services;

public class EmployeesServiceSynchronizationTests : IDisposable
{
    private readonly ResourceIdeaDBContext _dbContext;
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly EmployeesService _service;

    public EmployeesServiceSynchronizationTests()
    {
        var options = new DbContextOptionsBuilder<ResourceIdeaDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        
        _dbContext = new ResourceIdeaDBContext(options);
        
        _mockUserManager = MockUserManager();
        _service = new EmployeesService(_dbContext, _mockUserManager.Object);
    }

    [Fact]
    public async Task UpdateAsync_ValidEmployee_ShouldSynchronizeNamesWithApplicationUser()
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
            TenantId = tenantId,
            JobPositionId = JobPositionId.Create(Guid.NewGuid())
        };

        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.SaveChangesAsync();

        _mockUserManager
            .Setup(um => um.FindByIdAsync(applicationUserId.ToString()))
            .ReturnsAsync(applicationUser);

        _mockUserManager
            .Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>()))
            .Callback<ApplicationUser>(user => {
                user.FirstName = "Jane";
                user.LastName = "Smith";
            })
            .ReturnsAsync(IdentityResult.Success);

        // Create updated employee entity
        var updatedEmployee = new Employee
        {
            EmployeeId = employeeId,
            ApplicationUserId = applicationUserId,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "john.doe@example.com",
            TenantId = tenantId,
            JobPositionId = JobPositionId.Create(Guid.NewGuid())
        };

        // Act
        var result = await _service.UpdateAsync(updatedEmployee, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess, $"Expected success but got error: {result.Error}");
        Assert.True(result.Content.HasValue);

        // Verify that UserManager.UpdateAsync was called to sync ApplicationUser names
        _mockUserManager.Verify(um => um.FindByIdAsync(applicationUserId.ToString()), Times.Once);
        _mockUserManager.Verify(um => um.UpdateAsync(It.Is<ApplicationUser>(u => 
            u.FirstName == "Jane" && u.LastName == "Smith")), Times.Once);

        // Verify Employee record was updated
        var dbEmployee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
        Assert.NotNull(dbEmployee);
        Assert.Equal("Jane", dbEmployee.FirstName);
        Assert.Equal("Smith", dbEmployee.LastName);
    }

    [Fact]
    public async Task AddAsync_NewEmployee_ShouldCreateBothApplicationUserAndEmployee()
    {
        // Arrange
        var employee = new Employee
        {
            EmployeeId = EmployeeId.NewId(),
            ApplicationUserId = ApplicationUserId.Create(Guid.NewGuid()),
            FirstName = "Alice",
            LastName = "Johnson",
            Email = "alice.johnson@example.com",
            TenantId = TenantId.Create(Guid.NewGuid()),
            JobPositionId = JobPositionId.Create(Guid.NewGuid())
        };

        _mockUserManager
            .Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .Callback<ApplicationUser, string>((user, password) => {
                // Simulate successful user creation
                user.Id = employee.ApplicationUserId.ToString();
            })
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service.AddAsync(employee, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess, $"Expected success but got error: {result.Error}");
        Assert.True(result.Content.HasValue);

        // Verify ApplicationUser was created with correct names
        _mockUserManager.Verify(um => um.CreateAsync(It.Is<ApplicationUser>(u => 
            u.FirstName == "Alice" && 
            u.LastName == "Johnson" &&
            u.Email == "alice.johnson@example.com"), 
            It.IsAny<string>()), Times.Once);

        // Verify Employee record was created
        var dbEmployee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);
        Assert.NotNull(dbEmployee);
        Assert.Equal("Alice", dbEmployee.FirstName);
        Assert.Equal("Johnson", dbEmployee.LastName);
        Assert.Equal("alice.johnson@example.com", dbEmployee.Email);
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
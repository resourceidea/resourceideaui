using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.UnitTests.Employees.Models;

/// <summary>
/// Unit tests for <see cref="EmployeeModel"/>.
/// </summary>
public class EmployeeModelTests
{
    [Fact]
    public void IsValid_WithAllRequiredFieldsButNoManager_ShouldReturnTrue()
    {
        // Arrange
        var model = new EmployeeModel
        {
            EmployeeId = EmployeeId.Create(Guid.NewGuid()),
            JobPositionId = JobPositionId.Create(Guid.NewGuid()),
            DepartmentId = DepartmentId.Create(Guid.NewGuid()),
            ApplicationUserId = ApplicationUserId.Create(Guid.NewGuid()),
            EmployeeNumber = "EMP001",
            ManagerId = EmployeeId.Empty, // No manager - should be valid
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        // Act
        var result = model.IsValid();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationFailureMessages);
    }

    [Fact]
    public void IsValid_WithAllRequiredFieldsAndManager_ShouldReturnTrue()
    {
        // Arrange
        var model = new EmployeeModel
        {
            EmployeeId = EmployeeId.Create(Guid.NewGuid()),
            JobPositionId = JobPositionId.Create(Guid.NewGuid()),
            DepartmentId = DepartmentId.Create(Guid.NewGuid()),
            ApplicationUserId = ApplicationUserId.Create(Guid.NewGuid()),
            EmployeeNumber = "EMP001",
            ManagerId = EmployeeId.Create(Guid.NewGuid()), // Has manager
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        // Act
        var result = model.IsValid();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationFailureMessages);
    }

    [Fact]
    public void IsValid_WithMissingRequiredFields_ShouldReturnFalse()
    {
        // Arrange
        var model = new EmployeeModel
        {
            EmployeeId = EmployeeId.Empty, // Invalid
            JobPositionId = JobPositionId.Empty, // Invalid
            ApplicationUserId = ApplicationUserId.Empty, // Invalid
            EmployeeNumber = "", // Invalid
            FirstName = "", // Invalid
            LastName = "", // Invalid
            Email = "", // Invalid
            TenantId = TenantId.Empty // Invalid
            // ManagerId is not required, so Empty is okay
        };

        // Act
        var result = model.IsValid();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.ValidationFailureMessages);
        
        // The key thing is that ManagerId is optional - verify it's not in the error messages
        Assert.DoesNotContain(result.ValidationFailureMessages, msg => msg.Contains("ManagerId") || msg.Contains("Manager ID"));
    }

    [Fact]
    public void ToEntity_ShouldMapManagerIdToReportsTo()
    {
        // Arrange
        var managerId = EmployeeId.Create(Guid.NewGuid());
        var model = new EmployeeModel
        {
            EmployeeId = EmployeeId.Create(Guid.NewGuid()),
            JobPositionId = JobPositionId.Create(Guid.NewGuid()),
            ApplicationUserId = ApplicationUserId.Create(Guid.NewGuid()),
            EmployeeNumber = "EMP001",
            ManagerId = managerId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com"
        };

        // Act
        var entity = model.ToEntity();

        // Assert
        Assert.Equal(managerId, entity.ReportsTo);
        Assert.Equal(model.EmployeeId, entity.EmployeeId);
        Assert.Equal(model.JobPositionId, entity.JobPositionId);
        Assert.Equal(model.ApplicationUserId, entity.ApplicationUserId);
        Assert.Equal(model.EmployeeNumber, entity.EmployeeNumber);
    }
}
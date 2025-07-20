using EastSeat.ResourceIdea.Application.Features.Employees.Commands;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.Employees.Commands;

/// <summary>
/// Unit tests for <see cref="UpdateEmployeeCommand"/>.
/// </summary>
public class UpdateEmployeeCommandTests
{
    [Fact]
    public void ToEntity_ShouldMapAllPropertiesIncludingManagerId()
    {
        // Arrange
        var employeeId = EmployeeId.Create(Guid.NewGuid());
        var managerId = EmployeeId.Create(Guid.NewGuid());
        var jobPositionId = JobPositionId.Create(Guid.NewGuid());
        var departmentId = DepartmentId.Create(Guid.NewGuid());
        var applicationUserId = ApplicationUserId.Create(Guid.NewGuid());
        var tenantId = TenantId.Create(Guid.NewGuid());

        var command = new UpdateEmployeeCommand
        {
            EmployeeId = employeeId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            JobPositionId = jobPositionId,
            DepartmentId = departmentId,
            ApplicationUserId = applicationUserId,
            EmployeeNumber = "EMP001",
            ManagerId = managerId,
            TenantId = tenantId
        };

        // Act
        var entity = command.ToEntity();

        // Assert
        Assert.Equal(employeeId, entity.EmployeeId);
        Assert.Equal("John", entity.FirstName);
        Assert.Equal("Doe", entity.LastName);
        Assert.Equal("john.doe@test.com", entity.Email);
        Assert.Equal(jobPositionId, entity.JobPositionId);
        Assert.Equal(applicationUserId, entity.ApplicationUserId);
        Assert.Equal("EMP001", entity.EmployeeNumber);
        Assert.Equal(managerId, entity.ReportsTo); // Manager ID maps to ReportsTo
        Assert.Equal(tenantId, entity.TenantId);
    }

    [Fact]
    public void ToEntity_WithNoManagerId_ShouldMapToEmptyReportsTo()
    {
        // Arrange
        var command = new UpdateEmployeeCommand
        {
            EmployeeId = EmployeeId.Create(Guid.NewGuid()),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            JobPositionId = JobPositionId.Create(Guid.NewGuid()),
            ApplicationUserId = ApplicationUserId.Create(Guid.NewGuid()),
            EmployeeNumber = "EMP001",
            ManagerId = EmployeeId.Empty, // No manager
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        // Act
        var entity = command.ToEntity();

        // Assert
        Assert.Equal(EmployeeId.Empty, entity.ReportsTo);
    }

    [Fact]
    public void Validate_WithValidData_ShouldReturnSuccess()
    {
        // Arrange
        var command = new UpdateEmployeeCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com"
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationFailureMessages);
    }

    [Fact]
    public void Validate_WithMissingRequiredFields_ShouldReturnFailure()
    {
        // Arrange
        var command = new UpdateEmployeeCommand
        {
            FirstName = "", // Invalid
            LastName = "Doe",
            Email = "" // Invalid
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.ValidationFailureMessages);
        Assert.Contains(result.ValidationFailureMessages, msg => msg.Contains("FirstName"));
        Assert.Contains(result.ValidationFailureMessages, msg => msg.Contains("Email"));
    }
}
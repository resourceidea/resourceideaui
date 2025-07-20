// ===================================================================================
// File: TenantEmployeesSpecificationTests.cs
// Path: src/tests/EastSeat.ResourceIdea.Application.UnitTests/Features/Employees/Specifications/TenantEmployeesSpecificationTests.cs
// Description: Tests for TenantEmployeesSpecification to verify filtering logic.
// ===================================================================================

using EastSeat.ResourceIdea.Application.Features.Employees.Specifications;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using Xunit;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.Employees.Specifications;

/// <summary>
/// Tests for TenantEmployeesSpecification.
/// </summary>
public class TenantEmployeesSpecificationTests
{
    private readonly TenantId _tenantId;

    public TenantEmployeesSpecificationTests()
    {
        _tenantId = TenantId.Create(Guid.NewGuid());
    }

    [Fact]
    public void Criteria_ShouldIncludeActiveEmployee_WhenEndDateIsNull()
    {
        // Arrange
        var specification = new TenantEmployeesSpecification(_tenantId);
        var activeEmployee = new Employee
        {
            EmployeeId = EmployeeId.Create(Guid.NewGuid()),
            TenantId = _tenantId,
            EndDate = null
        };

        // Act
        var predicate = specification.Criteria.Compile();
        var result = predicate(activeEmployee);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Criteria_ShouldIncludeActiveEmployee_WhenEndDateIsInFuture()
    {
        // Arrange
        var specification = new TenantEmployeesSpecification(_tenantId);
        var activeEmployee = new Employee
        {
            EmployeeId = EmployeeId.Create(Guid.NewGuid()),
            TenantId = _tenantId,
            EndDate = DateTimeOffset.Now.AddDays(30) // 30 days in future
        };

        // Act
        var predicate = specification.Criteria.Compile();
        var result = predicate(activeEmployee);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Criteria_ShouldExcludeTerminatedEmployee_WhenEndDateIsInPast()
    {
        // Arrange
        var specification = new TenantEmployeesSpecification(_tenantId);
        var terminatedEmployee = new Employee
        {
            EmployeeId = EmployeeId.Create(Guid.NewGuid()),
            TenantId = _tenantId,
            EndDate = DateTimeOffset.Now.AddDays(-30) // 30 days ago
        };

        // Act
        var predicate = specification.Criteria.Compile();
        var result = predicate(terminatedEmployee);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Criteria_ShouldExcludeEmployee_WhenTenantIdIsWrong()
    {
        // Arrange
        var wrongTenantId = TenantId.Create(Guid.NewGuid());
        var specification = new TenantEmployeesSpecification(_tenantId);
        var employee = new Employee
        {
            EmployeeId = EmployeeId.Create(Guid.NewGuid()),
            TenantId = wrongTenantId, // Different tenant
            EndDate = null
        };

        // Act
        var predicate = specification.Criteria.Compile();
        var result = predicate(employee);

        // Assert
        Assert.False(result);
    }
}
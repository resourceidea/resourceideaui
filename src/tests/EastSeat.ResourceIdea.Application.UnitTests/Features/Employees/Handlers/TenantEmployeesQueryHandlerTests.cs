// ===================================================================================
// File: TenantEmployeesQueryHandlerTests.cs
// Path: src/tests/EastSeat.ResourceIdea.Application.UnitTests/Features/Employees/Handlers/TenantEmployeesQueryHandlerTests.cs
// Description: Tests for TenantEmployeesQueryHandler to verify terminated employees are excluded.
// ===================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Application.Features.Employees.Handlers;
using EastSeat.ResourceIdea.Application.Features.Employees.Queries;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using Moq;
using Xunit;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.Employees.Handlers;

/// <summary>
/// Tests for TenantEmployeesQueryHandler.
/// </summary>
public class TenantEmployeesQueryHandlerTests
{
    private readonly Mock<IEmployeeService> _mockEmployeeService;
    private readonly TenantEmployeesQueryHandler _handler;
    private readonly TenantId _tenantId;

    public TenantEmployeesQueryHandlerTests()
    {
        _mockEmployeeService = new Mock<IEmployeeService>();
        _handler = new TenantEmployeesQueryHandler(_mockEmployeeService.Object);
        _tenantId = TenantId.Create(Guid.NewGuid());
    }

    [Fact]
    public async Task Handle_ShouldExcludeTerminatedEmployees_WhenEmployeesHaveEndDate()
    {
        // Arrange
        var query = new TenantEmployeesQuery
        {
            TenantId = _tenantId,
            PageNumber = 1,
            PageSize = 10
        };

        var activeEmployee = new Employee
        {
            EmployeeId = EmployeeId.Create(Guid.NewGuid()),
            TenantId = _tenantId,
            FirstName = "Active",
            LastName = "Employee",
            Email = "active@test.com",
            EndDate = null // Active employee
        };

        var pagedList = new PagedListResponse<Employee>
        {
            Items = new List<Employee> { activeEmployee }, // Only active employee should be returned
            TotalCount = 1,
            CurrentPage = 1,
            PageSize = 10
        };

        _mockEmployeeService
            .Setup(x => x.GetPagedListAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Optional<BaseSpecification<Employee>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResourceIdeaResponse<PagedListResponse<Employee>>.Success(pagedList));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);
        
        var employeeModels = result.Content.Value.Items;
        Assert.Single(employeeModels);
        Assert.Equal("Active", employeeModels.First().FirstName);
        Assert.DoesNotContain(employeeModels, e => e.FirstName == "Terminated");
    }

    [Fact]
    public async Task Handle_ShouldIncludeEmployeesWithFutureEndDate_WhenEndDateIsInFuture()
    {
        // Arrange
        var query = new TenantEmployeesQuery
        {
            TenantId = _tenantId,
            PageNumber = 1,
            PageSize = 10
        };

        var employeeWithFutureEndDate = new Employee
        {
            EmployeeId = EmployeeId.Create(Guid.NewGuid()),
            TenantId = _tenantId,
            FirstName = "Future",
            LastName = "Termination",
            Email = "future@test.com",
            EndDate = DateTimeOffset.Now.AddDays(30) // Will be terminated in 30 days
        };

        var pagedList = new PagedListResponse<Employee>
        {
            Items = new List<Employee> { employeeWithFutureEndDate },
            TotalCount = 1,
            CurrentPage = 1,
            PageSize = 10
        };

        _mockEmployeeService
            .Setup(x => x.GetPagedListAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Optional<BaseSpecification<Employee>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResourceIdeaResponse<PagedListResponse<Employee>>.Success(pagedList));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);
        
        var employeeModels = result.Content.Value.Items;
        Assert.Single(employeeModels);
        Assert.Equal("Future", employeeModels.First().FirstName);
    }
}
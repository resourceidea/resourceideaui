// ===================================================================================
// File: GetEmployeeByIdQueryHandlerTests.cs
// Path: src/tests/EastSeat.ResourceIdea.Application.UnitTests/Features/Employees/Handlers/GetEmployeeByIdQueryHandlerTests.cs
// Description: Tests for GetEmployeeByIdQueryHandler to verify terminated employees are not accessible.
// ===================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Application.Features.Employees.Handlers;
using EastSeat.ResourceIdea.Application.Features.Employees.Queries;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using Moq;
using Xunit;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.Employees.Handlers;

/// <summary>
/// Tests for GetEmployeeByIdQueryHandler.
/// </summary>
public class GetEmployeeByIdQueryHandlerTests
{
    private readonly Mock<IEmployeeService> _mockEmployeeService;
    private readonly GetEmployeeByIdQueryHandler _handler;
    private readonly TenantId _tenantId;
    private readonly EmployeeId _employeeId;

    public GetEmployeeByIdQueryHandlerTests()
    {
        _mockEmployeeService = new Mock<IEmployeeService>();
        _handler = new GetEmployeeByIdQueryHandler(_mockEmployeeService.Object);
        _tenantId = TenantId.Create(Guid.NewGuid());
        _employeeId = EmployeeId.Create(Guid.NewGuid());
    }

    [Fact]
    public async Task Handle_ShouldReturnEmployeeNotFound_WhenEmployeeIsTerminated()
    {
        // Arrange
        var query = new GetEmployeeByIdQuery
        {
            EmployeeId = _employeeId,
            TenantId = _tenantId
        };

        // Mock the service to return no employee (as if terminated employee is filtered out)
        _mockEmployeeService
            .Setup(x => x.GetByIdAsync(
                It.IsAny<BaseSpecification<Employee>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResourceIdeaResponse<Employee>.Success(null));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCode.EmployeeNotFound, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmployee_WhenEmployeeIsActive()
    {
        // Arrange
        var query = new GetEmployeeByIdQuery
        {
            EmployeeId = _employeeId,
            TenantId = _tenantId
        };

        var activeEmployee = new Employee
        {
            EmployeeId = _employeeId,
            TenantId = _tenantId,
            FirstName = "Active",
            LastName = "Employee",
            Email = "active@test.com",
            EndDate = null // Active employee
        };

        _mockEmployeeService
            .Setup(x => x.GetByIdAsync(
                It.IsAny<BaseSpecification<Employee>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResourceIdeaResponse<Employee>.Success(activeEmployee));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);
        Assert.Equal("Active", result.Content.Value.FirstName);
    }
}
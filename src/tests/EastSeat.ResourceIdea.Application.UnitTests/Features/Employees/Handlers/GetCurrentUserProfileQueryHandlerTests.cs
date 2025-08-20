// ===================================================================================
// File: GetCurrentUserProfileQueryHandlerTests.cs
// Path: src/tests/EastSeat.ResourceIdea.Application.UnitTests/Features/Employees/Handlers/GetCurrentUserProfileQueryHandlerTests.cs
// Description: Tests for GetCurrentUserProfileQueryHandler.
// ===================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Application.Features.Employees.Handlers;
using EastSeat.ResourceIdea.Application.Features.Employees.Queries;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Departments.Entities;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using Moq;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.Employees.Handlers;

/// <summary>
/// Tests for GetCurrentUserProfileQueryHandler.
/// </summary>
public class GetCurrentUserProfileQueryHandlerTests
{
    private readonly Mock<IEmployeeService> _mockEmployeeService;
    private readonly GetCurrentUserProfileQueryHandler _handler;
    private readonly TenantId _tenantId;
    private readonly ApplicationUserId _applicationUserId;

    public GetCurrentUserProfileQueryHandlerTests()
    {
        _mockEmployeeService = new Mock<IEmployeeService>();
        _handler = new GetCurrentUserProfileQueryHandler(_mockEmployeeService.Object);
        _tenantId = TenantId.Create(Guid.NewGuid());
        _applicationUserId = ApplicationUserId.Create(Guid.NewGuid().ToString());
    }

    [Fact]
    public async Task Handle_ShouldReturnUserProfile_WhenEmployeeExists()
    {
        // Arrange
        var query = new GetCurrentUserProfileQuery
        {
            ApplicationUserId = _applicationUserId,
            TenantId = _tenantId
        };

        var mockDepartment = new Department
        {
            Id = DepartmentId.Create(Guid.NewGuid()),
            Name = "Engineering",
            TenantId = _tenantId
        };

        var mockJobPosition = new JobPosition
        {
            Id = JobPositionId.Create(Guid.NewGuid()),
            Title = "Software Engineer",
            Department = mockDepartment,
            TenantId = _tenantId
        };

        var mockEmployee = new Employee
        {
            EmployeeId = EmployeeId.Create(Guid.NewGuid()),
            ApplicationUserId = _applicationUserId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            EmployeeNumber = "EMP001",
            JobPosition = mockJobPosition,
            JobPositionId = mockJobPosition.Id,
            TenantId = _tenantId,
            EndDate = null
        };

        _mockEmployeeService
            .Setup(x => x.GetByIdAsync(It.IsAny<BaseSpecification<Employee>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResourceIdeaResponse<Employee>.Success(mockEmployee));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);

        var profile = result.Content.Value;
        Assert.Equal("John", profile.FirstName);
        Assert.Equal("Doe", profile.LastName);
        Assert.Equal("john.doe@example.com", profile.Email);
        Assert.Equal("EMP001", profile.EmployeeNumber);
        Assert.Equal("Software Engineer", profile.JobPositionTitle);
        Assert.Equal("Engineering", profile.DepartmentName);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmployeeNotFound_WhenEmployeeDoesNotExist()
    {
        // Arrange
        var query = new GetCurrentUserProfileQuery
        {
            ApplicationUserId = _applicationUserId,
            TenantId = _tenantId
        };

        _mockEmployeeService
            .Setup(x => x.GetByIdAsync(It.IsAny<BaseSpecification<Employee>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResourceIdeaResponse<Employee>.Failure(ErrorCode.EmployeeNotFound));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.EmployeeNotFound, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationFailure_WhenQueryIsInvalid()
    {
        // Arrange
        var query = new GetCurrentUserProfileQuery
        {
            ApplicationUserId = ApplicationUserId.Empty, // Invalid
            TenantId = _tenantId
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.EmployeeQueryValidationFailure, result.Error);
    }
}
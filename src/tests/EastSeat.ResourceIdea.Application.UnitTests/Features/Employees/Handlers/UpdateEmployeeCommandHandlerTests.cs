// ================================================================================================
// File: UpdateEmployeeCommandHandlerTests.cs
// Path: src/tests/EastSeat.ResourceIdea.Application.UnitTests/Features/Employees/Handlers/UpdateEmployeeCommandHandlerTests.cs
// Description: Unit tests for UpdateEmployeeCommandHandler to verify name synchronization.
// ================================================================================================

using EastSeat.ResourceIdea.Application.Features.Employees.Commands;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Application.Features.Employees.Handlers;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using Moq;
using Xunit;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.Employees.Handlers;

public class UpdateEmployeeCommandHandlerTests
{
    private readonly Mock<IEmployeeService> _mockEmployeeService;
    private readonly UpdateEmployeeCommandHandler _handler;

    public UpdateEmployeeCommandHandlerTests()
    {
        _mockEmployeeService = new Mock<IEmployeeService>();
        _handler = new UpdateEmployeeCommandHandler(_mockEmployeeService.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateEmployeeWithSyncedNames()
    {
        // Arrange
        var command = new UpdateEmployeeCommand
        {
            EmployeeId = EmployeeId.NewId(),
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            JobPositionId = JobPositionId.Create(Guid.NewGuid()),
            DepartmentId = DepartmentId.Create(Guid.NewGuid()),
            ApplicationUserId = ApplicationUserId.Create(Guid.NewGuid()),
            EmployeeNumber = "EMP-12345678",
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        var expectedEmployee = new Employee
        {
            EmployeeId = command.EmployeeId,
            JobPositionId = command.JobPositionId,
            ApplicationUserId = command.ApplicationUserId,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            TenantId = command.TenantId,
            EmployeeNumber = command.EmployeeNumber
        };

        _mockEmployeeService
            .Setup(s => s.UpdateAsync(It.Is<Employee>(e => 
                e.FirstName == command.FirstName && 
                e.LastName == command.LastName &&
                e.Email == command.Email &&
                e.EmployeeId == command.EmployeeId), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResourceIdeaResponse<Employee>.Success(Optional<Employee>.Some(expectedEmployee)));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);
        
        var employeeModel = result.Content.Value;
        Assert.Equal(command.FirstName, employeeModel.FirstName);
        Assert.Equal(command.LastName, employeeModel.LastName);
        Assert.Equal(command.Email, employeeModel.Email);
        Assert.Equal(command.EmployeeId, employeeModel.EmployeeId);

        _mockEmployeeService.Verify(s => s.UpdateAsync(
            It.Is<Employee>(e => 
                e.FirstName == command.FirstName && 
                e.LastName == command.LastName &&
                e.Email == command.Email &&
                e.EmployeeId == command.EmployeeId), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ShouldReturnValidationFailure()
    {
        // Arrange
        var command = new UpdateEmployeeCommand
        {
            EmployeeId = EmployeeId.NewId(),
            FirstName = "", // Invalid - empty
            LastName = "Smith",
            Email = "jane.smith@example.com",
            JobPositionId = JobPositionId.Create(Guid.NewGuid()),
            ApplicationUserId = ApplicationUserId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCode.CommandValidationFailure, result.Error);
        
        _mockEmployeeService.Verify(s => s.UpdateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ServiceFailure_ShouldReturnServiceError()
    {
        // Arrange
        var command = new UpdateEmployeeCommand
        {
            EmployeeId = EmployeeId.NewId(),
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            JobPositionId = JobPositionId.Create(Guid.NewGuid()),
            ApplicationUserId = ApplicationUserId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        _mockEmployeeService
            .Setup(s => s.UpdateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResourceIdeaResponse<Employee>.Failure(ErrorCode.DbUpdateFailureOnUpdateEmployee));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCode.DbUpdateFailureOnUpdateEmployee, result.Error);
    }
}
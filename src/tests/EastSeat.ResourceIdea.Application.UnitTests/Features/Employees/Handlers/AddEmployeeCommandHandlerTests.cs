// ================================================================================================
// File: AddEmployeeCommandHandlerTests.cs
// Path: src/tests/EastSeat.ResourceIdea.Application.UnitTests/Features/Employees/Handlers/AddEmployeeCommandHandlerTests.cs
// Description: Unit tests for AddEmployeeCommandHandler to verify name synchronization.
// ================================================================================================

using EastSeat.ResourceIdea.Application.Features.Employees.Commands;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Application.Features.Employees.Handlers;
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

public class AddEmployeeCommandHandlerTests
{
    private readonly Mock<IEmployeeService> _mockEmployeeService;
    private readonly AddEmployeeCommandHandler _handler;

    public AddEmployeeCommandHandlerTests()
    {
        _mockEmployeeService = new Mock<IEmployeeService>();
        _handler = new AddEmployeeCommandHandler(_mockEmployeeService.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateEmployeeWithSyncedNames()
    {
        // Arrange
        var command = new AddEmployeeCommand
        {
            JobPositionId = JobPositionId.Create(Guid.NewGuid()),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        var expectedEmployee = new Employee
        {
            EmployeeId = EmployeeId.NewId(),
            JobPositionId = command.JobPositionId,
            ApplicationUserId = ApplicationUserId.Create(Guid.NewGuid()),
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            TenantId = command.TenantId
        };

        _mockEmployeeService
            .Setup(s => s.AddAsync(It.Is<Employee>(e => 
                e.FirstName == command.FirstName && 
                e.LastName == command.LastName &&
                e.Email == command.Email), 
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

        _mockEmployeeService.Verify(s => s.AddAsync(
            It.Is<Employee>(e => 
                e.FirstName == command.FirstName && 
                e.LastName == command.LastName &&
                e.Email == command.Email), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ShouldReturnValidationFailure()
    {
        // Arrange
        var command = new AddEmployeeCommand
        {
            JobPositionId = JobPositionId.Empty,
            FirstName = "", // Invalid - empty
            LastName = "Doe",
            Email = "john.doe@example.com",
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCode.CommandValidationFailure, result.Error);
        
        _mockEmployeeService.Verify(s => s.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ServiceFailure_ShouldReturnServiceError()
    {
        // Arrange
        var command = new AddEmployeeCommand
        {
            JobPositionId = JobPositionId.Create(Guid.NewGuid()),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        _mockEmployeeService
            .Setup(s => s.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResourceIdeaResponse<Employee>.Failure(ErrorCode.AddApplicationUserFailure));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCode.AddApplicationUserFailure, result.Error);
    }
}
using EastSeat.ResourceIdea.Application.Features.Employees.Commands;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Application.Features.Employees.Handlers;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

using Moq;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.Employees.Handlers;

/// <summary>
/// Unit tests for <see cref="ResetEmployeePasswordCommandHandler"/>.
/// </summary>
public class ResetEmployeePasswordCommandHandlerTests
{
    private readonly Mock<IApplicationUserService> _mockApplicationUserService;
    private readonly ResetEmployeePasswordCommandHandler _handler;

    public ResetEmployeePasswordCommandHandlerTests()
    {
        _mockApplicationUserService = new Mock<IApplicationUserService>();
        _handler = new ResetEmployeePasswordCommandHandler(_mockApplicationUserService.Object);
    }

    [Fact]
    public async Task Handle_WhenServiceReturnsSuccess_ShouldReturnSuccessResponse()
    {
        // Arrange
        var command = new ResetEmployeePasswordCommand
        {
            EmployeeId = EmployeeId.Create(Guid.NewGuid()),
            Email = "test@example.com",
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        var expectedTemporaryPassword = "Temp@12345678";
        var serviceResponse = ResourceIdeaResponse<string>.Success(expectedTemporaryPassword);

        _mockApplicationUserService
            .Setup(s => s.ResetPasswordAsync(command.Email))
            .ReturnsAsync(serviceResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedTemporaryPassword, result.Content.Value);
        _mockApplicationUserService.Verify(s => s.ResetPasswordAsync(command.Email), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenServiceReturnsFailure_ShouldReturnFailureResponse()
    {
        // Arrange
        var command = new ResetEmployeePasswordCommand
        {
            EmployeeId = EmployeeId.Create(Guid.NewGuid()),
            Email = "test@example.com",
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        var serviceResponse = ResourceIdeaResponse<string>.Failure(ErrorCode.UserNotFound);

        _mockApplicationUserService
            .Setup(s => s.ResetPasswordAsync(command.Email))
            .ReturnsAsync(serviceResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCode.UserNotFound, result.Error);
        _mockApplicationUserService.Verify(s => s.ResetPasswordAsync(command.Email), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenValidationFails_ShouldReturnValidationFailure()
    {
        // Arrange
        var command = new ResetEmployeePasswordCommand
        {
            EmployeeId = EmployeeId.Create(Guid.NewGuid()),
            Email = string.Empty, // Invalid email
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCode.CommandValidationFailure, result.Error);
        _mockApplicationUserService.Verify(s => s.ResetPasswordAsync(It.IsAny<string>()), Times.Never);
    }
}
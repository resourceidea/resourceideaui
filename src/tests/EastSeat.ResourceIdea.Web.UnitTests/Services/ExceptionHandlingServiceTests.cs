using EastSeat.ResourceIdea.Domain.Exceptions;
using EastSeat.ResourceIdea.Web.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EastSeat.ResourceIdea.Web.UnitTests.Services;

public class ExceptionHandlingServiceTests
{
    private readonly Mock<ILogger<ExceptionHandlingService>> _loggerMock;
    private readonly ExceptionHandlingService _service;

    public ExceptionHandlingServiceTests()
    {
        _loggerMock = new Mock<ILogger<ExceptionHandlingService>>();
        _service = new ExceptionHandlingService(_loggerMock.Object);
    }

    [Fact]
    public async Task HandleExceptionAsync_TaskCanceledException_ReturnsUserFriendlyMessage()
    {
        // Arrange
        var exception = new TaskCanceledException("Test message");

        // Act
        var result = await _service.HandleExceptionAsync(exception, "Test context");

        // Assert
        Assert.Equal("The operation was canceled. Please try again.", result);
    }

    [Fact]
    public async Task HandleExceptionAsync_ArgumentException_ReturnsUserFriendlyMessage()
    {
        // Arrange
        var exception = new ArgumentException("Test message");

        // Act
        var result = await _service.HandleExceptionAsync(exception, "Test context");

        // Assert
        Assert.Equal("An error occurred due to invalid input. Please check your parameters.", result);
    }

    [Fact]
    public async Task HandleExceptionAsync_InvalidEntityIdException_ReturnsUserFriendlyMessage()
    {
        // Arrange
        var exception = new InvalidEntityIdException("Test message");

        // Act
        var result = await _service.HandleExceptionAsync(exception, "Test context");

        // Assert
        Assert.Equal("The provided identifier is invalid.", result);
    }

    [Fact]
    public async Task HandleExceptionAsync_GenericException_ReturnsGenericMessage()
    {
        // Arrange
        var exception = new Exception("Test message");

        // Act
        var result = await _service.HandleExceptionAsync(exception, "Test context");

        // Assert
        Assert.Equal("An unexpected error occurred. Please try again or contact support if the problem persists.", result);
    }

    [Fact]
    public async Task ExecuteAsync_SuccessfulOperation_ReturnsSuccess()
    {
        // Arrange
        var wasExecuted = false;
        Func<Task> operation = () =>
        {
            wasExecuted = true;
            return Task.CompletedTask;
        };

        // Act
        var result = await _service.ExecuteAsync(operation, "Test context");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(wasExecuted);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_OperationThrowsException_ReturnsFailure()
    {
        // Arrange
        var exception = new ArgumentException("Test exception");
        Func<Task> operation = () => throw exception;

        // Act
        var result = await _service.ExecuteAsync(operation, "Test context");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.ErrorMessage);
        Assert.Equal("An error occurred due to invalid input. Please check your parameters.", result.ErrorMessage);
        Assert.Equal(exception, result.Exception);
    }

    [Fact]
    public async Task ExecuteAsync_WithResult_SuccessfulOperation_ReturnsSuccessWithData()
    {
        // Arrange
        var expectedResult = "Test Result";
        Func<Task<string>> operation = () => Task.FromResult(expectedResult);

        // Act
        var result = await _service.ExecuteAsync(operation, "Test context");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedResult, result.Data);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_WithResult_OperationThrowsException_ReturnsFailure()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");
        Func<Task<string>> operation = () => throw exception;

        // Act
        var result = await _service.ExecuteAsync(operation, "Test context");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Data);
        Assert.NotNull(result.ErrorMessage);
        Assert.Equal("An invalid operation occurred. Please try again.", result.ErrorMessage);
        Assert.Equal(exception, result.Exception);
    }
}
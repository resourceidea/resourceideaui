// ----------------------------------------------------------------------------------
// File: LogoutCommandTests.cs
// Path: src\tests\EastSeat.ResourceIdea.Application.UnitTests\Features\Authentication\Commands\LogoutCommandTests.cs
// Description: Unit tests for LogoutCommand validation.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Authentication.Commands;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.Authentication.Commands;

public class LogoutCommandTests
{
    [Fact]
    public void Validate_ShouldAlwaysReturnSuccess()
    {
        // Arrange
        var command = new LogoutCommand
        {
            ReturnUrl = "/home"
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationFailureMessages);
    }

    [Fact]
    public void Validate_WhenReturnUrlIsEmpty_ShouldStillReturnSuccess()
    {
        // Arrange
        var command = new LogoutCommand
        {
            ReturnUrl = string.Empty
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationFailureMessages);
    }

    [Fact]
    public void Validate_WhenReturnUrlIsNull_ShouldStillReturnSuccess()
    {
        // Arrange
        var command = new LogoutCommand
        {
            ReturnUrl = null
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationFailureMessages);
    }
}
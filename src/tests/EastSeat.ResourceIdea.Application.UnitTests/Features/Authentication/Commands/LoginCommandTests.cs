// ----------------------------------------------------------------------------------
// File: LoginCommandTests.cs
// Path: src\tests\EastSeat.ResourceIdea.Application.UnitTests\Features\Authentication\Commands\LoginCommandTests.cs
// Description: Unit tests for LoginCommand validation.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Authentication.Commands;
using Xunit;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.Authentication.Commands;

public class LoginCommandTests
{
    [Fact]
    public void Validate_WhenEmailIsEmpty_ShouldReturnValidationFailure()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = string.Empty,
            Password = "password123"
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("Email", string.Join(" ", result.ValidationFailureMessages));
    }

    [Fact]
    public void Validate_WhenPasswordIsEmpty_ShouldReturnValidationFailure()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "user@example.com",
            Password = string.Empty
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("Password", string.Join(" ", result.ValidationFailureMessages));
    }

    [Fact]
    public void Validate_WhenEmailFormatIsInvalid_ShouldReturnValidationFailure()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "invalid-email",
            Password = "password123"
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("Email address format is invalid", string.Join(" ", result.ValidationFailureMessages));
    }

    [Fact]
    public void Validate_WhenAllFieldsAreValid_ShouldReturnSuccess()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "user@example.com",
            Password = "password123",
            RememberMe = true,
            ReturnUrl = "/dashboard"
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationFailureMessages);
    }

    [Theory]
    [InlineData("user@example.com")]
    [InlineData("test.user@domain.org")]
    [InlineData("admin@company.co.uk")]
    public void Validate_WhenEmailFormatIsValid_ShouldPass(string email)
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = email,
            Password = "password123"
        };

        // Act
        var result = command.Validate();

        // Assert
        Assert.True(result.IsValid);
    }
}
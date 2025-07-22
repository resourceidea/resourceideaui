using EastSeat.ResourceIdea.Application.Extensions;

namespace EastSeat.ResourceIdea.Application.UnitTests.Extensions;

/// <summary>
/// Unit tests for <see cref="StringExtensions"/>.
/// </summary>
public class StringExtensionsTests
{
    [Fact]
    public void ThrowIfNullOrEmptyOrWhiteSpace_WithNull_ShouldThrowArgumentException()
    {
        // Arrange
        string? nullString = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullString.ThrowIfNullOrEmptyOrWhiteSpace());
    }

    [Fact]
    public void ThrowIfNullOrEmptyOrWhiteSpace_WithEmptyString_ShouldThrowArgumentException()
    {
        // Arrange
        string emptyString = string.Empty;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => emptyString.ThrowIfNullOrEmptyOrWhiteSpace());
    }

    [Fact]
    public void ThrowIfNullOrEmptyOrWhiteSpace_WithWhiteSpaceString_ShouldThrowArgumentException()
    {
        // Arrange
        string whiteSpaceString = "   ";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => whiteSpaceString.ThrowIfNullOrEmptyOrWhiteSpace());
    }

    [Fact]
    public void ThrowIfNullOrEmptyOrWhiteSpace_WithValidString_ShouldNotThrow()
    {
        // Arrange
        string validString = "Valid String";

        // Act & Assert
        var exception = Record.Exception(() => validString.ThrowIfNullOrEmptyOrWhiteSpace());
        Assert.Null(exception);
    }

    [Fact]
    public void ThrowIfNullOrEmptyOrWhiteSpace_WithStringContainingOnlySpaces_ShouldThrowArgumentException()
    {
        // Arrange
        string spacesOnlyString = "     ";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => spacesOnlyString.ThrowIfNullOrEmptyOrWhiteSpace());
    }

    [Fact]
    public void ThrowIfNullOrEmptyOrWhiteSpace_WithStringContainingTabsAndNewlines_ShouldThrowArgumentException()
    {
        // Arrange
        string whiteSpaceString = "\t\n\r";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => whiteSpaceString.ThrowIfNullOrEmptyOrWhiteSpace());
    }
}
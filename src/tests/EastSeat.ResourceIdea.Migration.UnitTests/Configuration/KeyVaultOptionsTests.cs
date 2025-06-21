using EastSeat.ResourceIdea.Migration.Configuration;
using FluentAssertions;
using Xunit;

namespace EastSeat.ResourceIdea.Migration.UnitTests.Configuration;

/// <summary>
/// Unit tests for <see cref="KeyVaultOptions"/>.
/// </summary>
public sealed class KeyVaultOptionsTests
{
    /// <summary>
    /// Tests that default values are set correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithDefaultValues_ShouldSetCorrectDefaults()
    {
        // Act
        var options = new KeyVaultOptions();

        // Assert
        options.VaultUri.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that section name is correct.
    /// </summary>
    [Fact]
    public void SectionName_ShouldBeCorrect()
    {
        // Act & Assert
        KeyVaultOptions.SectionName.Should().Be("KeyVault");
    }

    /// <summary>
    /// Tests that VaultUri property can be set correctly.
    /// </summary>
    [Fact]
    public void VaultUri_CanBeSetCorrectly()
    {
        // Arrange
        var expectedUri = "https://test-keyvault.vault.azure.net/";

        // Act
        var options = new KeyVaultOptions
        {
            VaultUri = expectedUri
        };

        // Assert
        options.VaultUri.Should().Be(expectedUri);
    }
}

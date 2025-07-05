using EastSeat.ResourceIdea.Migration.Configuration;
using EastSeat.ResourceIdea.Migration.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace EastSeat.ResourceIdea.Migration.UnitTests.Services;

/// <summary>
/// Unit tests for <see cref="ConnectionStringService"/>.
/// </summary>
public sealed class ConnectionStringServiceTests : IDisposable
{
    private readonly Mock<ILogger<ConnectionStringService>> _loggerMock;
    private readonly Mock<IOptions<KeyVaultOptions>> _optionsMock;
    private readonly KeyVaultOptions _keyVaultOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionStringServiceTests"/> class.
    /// </summary>
    public ConnectionStringServiceTests()
    {
        _loggerMock = new Mock<ILogger<ConnectionStringService>>();
        _optionsMock = new Mock<IOptions<KeyVaultOptions>>();
        _keyVaultOptions = new KeyVaultOptions
        {
            VaultUri = "https://test-keyvault.vault.azure.net/"
        };
        _optionsMock.Setup(x => x.Value).Returns(_keyVaultOptions);
    }

    /// <summary>
    /// Tests that the constructor initializes the service correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithValidOptions_ShouldInitializeSuccessfully()
    {
        // Act & Assert
        var service = new ConnectionStringService(_optionsMock.Object, _loggerMock.Object);
        service.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that the constructor throws when options are null.
    /// </summary>
    [Fact]
    public void Constructor_WithNullOptions_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var act = () => new ConnectionStringService(null!, _loggerMock.Object);
        act.Should().Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Tests that the constructor throws when logger is null.
    /// </summary>
    [Fact]
    public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var act = () => new ConnectionStringService(_optionsMock.Object, null!);
        act.Should().Throw<ArgumentNullException>();
    }
    
    /// <summary>
         /// Tests that the constructor throws when Key Vault URI is empty.
         /// </summary>
    [Fact]
    public void Constructor_WithEmptyVaultUri_ShouldThrowArgumentException()
    {
        // Arrange
        _keyVaultOptions.VaultUri = string.Empty;

        // Act & Assert
        var act = () => new ConnectionStringService(_optionsMock.Object, _loggerMock.Object);
        act.Should().Throw<ArgumentException>();
    }

    /// <summary>
    /// Tests that the constructor throws when Key Vault URI is invalid.
    /// </summary>
    [Fact]
    public void Constructor_WithInvalidVaultUri_ShouldThrowUriFormatException()
    {
        // Arrange
        _keyVaultOptions.VaultUri = "invalid-uri";

        // Act & Assert
        var act = () => new ConnectionStringService(_optionsMock.Object, _loggerMock.Object);
        act.Should().Throw<UriFormatException>();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // Clean up test resources if needed
    }
}

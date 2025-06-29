using EastSeat.ResourceIdea.Migration.Configuration;
using EastSeat.ResourceIdea.Migration.Models;
using EastSeat.ResourceIdea.Migration.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace EastSeat.ResourceIdea.Migration.UnitTests.Services;

/// <summary>
/// Unit tests for <see cref="DatabaseMigrationService"/>.
/// </summary>
public sealed class DatabaseMigrationServiceTests : IDisposable
{
    private readonly Mock<IConnectionStringService> _connectionStringServiceMock;
    private readonly Mock<ILogger<DatabaseMigrationService>> _loggerMock;
    private readonly Mock<IOptions<MigrationOptions>> _optionsMock;
    private readonly MigrationOptions _migrationOptions;
    private readonly DatabaseMigrationService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseMigrationServiceTests"/> class.
    /// </summary>
    public DatabaseMigrationServiceTests()
    {
        _connectionStringServiceMock = new Mock<IConnectionStringService>();
        _loggerMock = new Mock<ILogger<DatabaseMigrationService>>();
        _optionsMock = new Mock<IOptions<MigrationOptions>>();
        _migrationOptions = new MigrationOptions
        {
            BatchSize = 1000,
            CommandTimeoutSeconds = 300,
            MaxRetryAttempts = 3,
            RetryDelaySeconds = 5,
            EnableDetailedLogging = true
        };
        _optionsMock.Setup(x => x.Value).Returns(_migrationOptions);

        _service = new DatabaseMigrationService(
            _connectionStringServiceMock.Object,
            _optionsMock.Object,
            _loggerMock.Object);
    }

    /// <summary>
    /// Tests that the constructor initializes the service correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithValidParameters_ShouldInitializeSuccessfully()
    {
        // Act & Assert
        _service.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that the constructor throws when connection string service is null.
    /// </summary>
    [Fact]
    public void Constructor_WithNullConnectionStringService_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var act = () => new DatabaseMigrationService(
            null!,
            _optionsMock.Object,
            _loggerMock.Object);
        act.Should().Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Tests that the constructor throws when options are null.
    /// </summary>
    [Fact]
    public void Constructor_WithNullOptions_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var act = () => new DatabaseMigrationService(
            _connectionStringServiceMock.Object,
            null!,
            _loggerMock.Object);
        act.Should().Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Tests that the constructor throws when logger is null.
    /// </summary>
    [Fact]
    public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var act = () => new DatabaseMigrationService(
            _connectionStringServiceMock.Object,
            _optionsMock.Object,
            null!);
        act.Should().Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Tests that MigrateTableAsync with null table name throws ArgumentException.
    /// </summary>
    [Fact]
    public async Task MigrateTableAsync_WithNullTableName_ShouldReturnFailureResult()
    {
        // Act
        var result = await _service.MigrateTableAsync(null!, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.TableName.Should().BeEmpty();
        result.RecordsProcessed.Should().Be(0);
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    /// <summary>
    /// Tests that MigrateTableAsync with empty table name throws ArgumentException.
    /// </summary>
    [Fact]
    public async Task MigrateTableAsync_WithEmptyTableName_ShouldReturnFailureResult()
    {
        // Act
        var result = await _service.MigrateTableAsync(string.Empty, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.TableName.Should().BeEmpty();
        result.RecordsProcessed.Should().Be(0);
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    /// <summary>
    /// Tests that cancellation token is respected during migration.
    /// </summary>
    [Fact]
    public async Task MigrateTableAsync_WithCancelledToken_ShouldReturnCancelledResult()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        var result = await _service.MigrateTableAsync("TestTable", cts.Token);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.TableName.Should().Be("TestTable");
        result.RecordsProcessed.Should().Be(0);
    }    /// <inheritdoc />
    public void Dispose()
    {
        // Clean up test resources if needed
    }
}

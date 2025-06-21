using EastSeat.ResourceIdea.Migration.Models;
using FluentAssertions;
using Xunit;

namespace EastSeat.ResourceIdea.Migration.UnitTests.Models;

/// <summary>
/// Unit tests for <see cref="MigrationResult"/>.
/// </summary>
public sealed class MigrationResultTests
{
    /// <summary>
    /// Tests that Duration calculation works correctly.
    /// </summary>
    [Fact]
    public void Duration_WithValidStartAndEndTime_ShouldCalculateCorrectDuration()
    {
        // Arrange
        var startTime = DateTime.UtcNow;
        var endTime = startTime.AddMinutes(5);
        var result = new MigrationResult
        {
            StartTime = startTime,
            EndTime = endTime
        };

        // Act
        var duration = result.Duration;

        // Assert
        duration.Should().Be(TimeSpan.FromMinutes(5));
    }

    /// <summary>
    /// Tests that ToString method returns correct format for successful migration.
    /// </summary>
    [Fact]
    public void ToString_WithSuccessfulMigration_ShouldReturnCorrectFormat()
    {
        // Arrange
        var result = new MigrationResult
        {
            TableName = "TestTable",
            Success = true,
            RecordsProcessed = 1000,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddMilliseconds(500)
        };

        // Act
        var stringResult = result.ToString();

        // Assert
        stringResult.Should().StartWith("[SUCCESS] TestTable: 1000 records in 500.0ms");
    }

    /// <summary>
    /// Tests that ToString method returns correct format for failed migration.
    /// </summary>
    [Fact]
    public void ToString_WithFailedMigration_ShouldReturnCorrectFormat()
    {
        // Arrange
        var result = new MigrationResult
        {
            TableName = "TestTable",
            Success = false,
            RecordsProcessed = 0,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddMilliseconds(100),
            ErrorMessage = "Connection failed"
        };

        // Act
        var stringResult = result.ToString();

        // Assert
        stringResult.Should().StartWith("[FAILED] TestTable: 0 records in 100.0ms - Connection failed");
    }

    /// <summary>
    /// Tests that ToString method works correctly without error message.
    /// </summary>
    [Fact]
    public void ToString_WithFailedMigrationWithoutErrorMessage_ShouldReturnCorrectFormat()
    {
        // Arrange
        var result = new MigrationResult
        {
            TableName = "TestTable",
            Success = false,
            RecordsProcessed = 500,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddMilliseconds(250)
        };

        // Act
        var stringResult = result.ToString();

        // Assert
        stringResult.Should().StartWith("[FAILED] TestTable: 500 records in 250.0ms");
        stringResult.Should().NotContain(" - ");
    }

    /// <summary>
    /// Tests that default values are set correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithDefaultValues_ShouldSetCorrectDefaults()
    {
        // Act
        var result = new MigrationResult();

        // Assert
        result.TableName.Should().BeEmpty();
        result.Success.Should().BeFalse();
        result.RecordsProcessed.Should().Be(0);
        result.StartTime.Should().Be(default);
        result.EndTime.Should().Be(default);
        result.ErrorMessage.Should().BeNull();
    }
}

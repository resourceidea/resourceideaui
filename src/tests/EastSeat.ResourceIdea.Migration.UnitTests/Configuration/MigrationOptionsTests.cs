using EastSeat.ResourceIdea.Migration.Configuration;
using FluentAssertions;
using Xunit;

namespace EastSeat.ResourceIdea.Migration.UnitTests.Configuration;

/// <summary>
/// Unit tests for <see cref="MigrationOptions"/>.
/// </summary>
public sealed class MigrationOptionsTests
{
    /// <summary>
    /// Tests that default values are set correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithDefaultValues_ShouldSetCorrectDefaults()
    {
        // Act
        var options = new MigrationOptions();

        // Assert
        options.BatchSize.Should().Be(1000);
        options.CommandTimeoutSeconds.Should().Be(300);
        options.MaxRetryAttempts.Should().Be(3);
        options.RetryDelaySeconds.Should().Be(5);
        options.EnableDetailedLogging.Should().BeTrue();
    }

    /// <summary>
    /// Tests that section name is correct.
    /// </summary>
    [Fact]
    public void SectionName_ShouldBeCorrect()
    {
        // Act & Assert
        MigrationOptions.SectionName.Should().Be("Migration");
    }

    /// <summary>
    /// Tests that properties can be set correctly.
    /// </summary>
    [Fact]
    public void Properties_CanBeSetCorrectly()
    {
        // Act
        var options = new MigrationOptions
        {
            BatchSize = 2000,
            CommandTimeoutSeconds = 600,
            MaxRetryAttempts = 5,
            RetryDelaySeconds = 10,
            EnableDetailedLogging = false
        };

        // Assert
        options.BatchSize.Should().Be(2000);
        options.CommandTimeoutSeconds.Should().Be(600);
        options.MaxRetryAttempts.Should().Be(5);
        options.RetryDelaySeconds.Should().Be(10);
        options.EnableDetailedLogging.Should().BeFalse();
    }
}

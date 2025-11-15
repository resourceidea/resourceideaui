using EastSeat.ResourceIdea.Domain.Entities;
using Xunit;

namespace EastSeat.ResourceIdea.Domain.Tests.Entities;

public class ClientTests
{
    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        // Arrange
        var name = "ABC Corporation";
        var registrationNumber = "REG123";
        var sector = "Manufacturing";

        // Act
        var result = Client.Create(name, registrationNumber, sector);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(name, result.Value.Name);
        Assert.Equal(registrationNumber, result.Value.RegistrationNumber);
        Assert.True(result.Value.IsActive);
    }

    [Fact]
    public void Create_WithEmptyName_ShouldFail()
    {
        // Act
        var result = Client.Create(string.Empty);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var client = Client.Create("Test Client").Value!;

        // Act
        client.Deactivate();

        // Assert
        Assert.False(client.IsActive);
    }
}

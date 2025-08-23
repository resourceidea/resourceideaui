// ===================================================================================
// File: GetCurrentUserProfileQueryTests.cs
// Path: src/tests/EastSeat.ResourceIdea.Application.UnitTests/Features/Employees/Queries/GetCurrentUserProfileQueryTests.cs
// Description: Tests for GetCurrentUserProfileQuery validation.
// ===================================================================================

using EastSeat.ResourceIdea.Application.Features.Employees.Queries;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.Employees.Queries;

/// <summary>
/// Tests for GetCurrentUserProfileQuery.
/// </summary>
public class GetCurrentUserProfileQueryTests
{
    [Fact]
    public void Validate_ShouldReturnValid_WhenAllPropertiesAreSet()
    {
        // Arrange
        var query = new GetCurrentUserProfileQuery
        {
            ApplicationUserId = ApplicationUserId.Create(Guid.NewGuid().ToString()),
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationFailureMessages);
    }

    [Fact]
    public void Validate_ShouldReturnInvalid_WhenApplicationUserIdIsEmpty()
    {
        // Arrange
        var query = new GetCurrentUserProfileQuery
        {
            ApplicationUserId = ApplicationUserId.Empty,
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.ValidationFailureMessages);
    }

    [Fact]
    public void Validate_ShouldReturnInvalid_WhenTenantIdIsEmpty()
    {
        // Arrange
        var query = new GetCurrentUserProfileQuery
        {
            ApplicationUserId = ApplicationUserId.Create(Guid.NewGuid().ToString()),
            TenantId = TenantId.Empty
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.ValidationFailureMessages);
    }
}
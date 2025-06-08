using EastSeat.ResourceIdea.Application.Features.Engagements.Queries;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Exceptions;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.Engagements.Queries;

/// <summary>
/// Unit tests for <see cref="GetEngagementsByClientQuery"/> validation.
/// </summary>
public class GetEngagementsByClientQueryTests
{
    [Fact]
    public void Validate_WhenValidQuery_ShouldReturnValidResponse()
    {
        // Arrange
        var query = new GetEngagementsByClientQuery(1, 10)
        {
            ClientId = ClientId.Create(Guid.NewGuid()),
            SearchTerm = "Test",
            SortField = "name",
            SortDirection = "asc"
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationFailureMessages);
    }

    [Fact]
    public void Validate_WhenClientIdIsEmpty_ShouldThrowInvalidEntityIdException()
    {
        Assert.Throws<InvalidEntityIdException>(() => new GetEngagementsByClientQuery(1, 10)
        {
            ClientId = ClientId.Create(Guid.Empty)
        });
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Validate_WhenPageNumberIsInvalid_ShouldReturnInvalidResponse(int pageNumber)
    {
        // Arrange
        var query = new GetEngagementsByClientQuery(pageNumber, 10)
        {
            ClientId = ClientId.Create(Guid.NewGuid())
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.ValidationFailureMessages, msg => msg.Contains("Page number must be greater than or equal to 1"));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(100)]
    public void Validate_WhenPageNumberIsValid_ShouldNotReturnPageNumberError(int pageNumber)
    {
        // Arrange
        var query = new GetEngagementsByClientQuery(pageNumber, 10)
        {
            ClientId = ClientId.Create(Guid.NewGuid())
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.DoesNotContain(result.ValidationFailureMessages, msg => msg.Contains("Page number"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    [InlineData(200)]
    public void Validate_WhenPageSizeIsInvalid_ShouldReturnInvalidResponse(int pageSize)
    {
        // Arrange
        var query = new GetEngagementsByClientQuery(1, pageSize)
        {
            ClientId = ClientId.Create(Guid.NewGuid())
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.ValidationFailureMessages, msg =>
            msg.Contains("Page size must be greater than or equal to 1") ||
            msg.Contains("Page size cannot exceed 100 items"));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    public void Validate_WhenPageSizeIsValid_ShouldNotReturnPageSizeError(int pageSize)
    {
        // Arrange
        var query = new GetEngagementsByClientQuery(1, pageSize)
        {
            ClientId = ClientId.Create(Guid.NewGuid())
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.DoesNotContain(result.ValidationFailureMessages, msg => msg.Contains("Page size"));
    }

    [Theory]
    [InlineData("asc")]
    [InlineData("desc")]
    [InlineData("ASC")]
    [InlineData("DESC")]
    [InlineData("Asc")]
    [InlineData("Desc")]
    public void Validate_WhenSortDirectionIsValid_ShouldNotReturnSortDirectionError(string sortDirection)
    {
        // Arrange
        var query = new GetEngagementsByClientQuery(1, 10)
        {
            ClientId = ClientId.Create(Guid.NewGuid()),
            SortDirection = sortDirection
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.DoesNotContain(result.ValidationFailureMessages, msg => msg.Contains("Sort direction"));
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("ascending")]
    [InlineData("descending")]
    [InlineData("up")]
    [InlineData("down")]
    public void Validate_WhenSortDirectionIsInvalid_ShouldReturnInvalidResponse(string sortDirection)
    {
        // Arrange
        var query = new GetEngagementsByClientQuery(1, 10)
        {
            ClientId = ClientId.Create(Guid.NewGuid()),
            SortDirection = sortDirection
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.ValidationFailureMessages, msg => msg.Contains("Sort direction must be either 'asc' or 'desc'"));
    }

    [Theory]
    [InlineData("name")]
    [InlineData("startdate")]
    [InlineData("enddate")]
    [InlineData("status")]
    [InlineData("createdat")]
    [InlineData("NAME")]
    [InlineData("StartDate")]
    [InlineData("EndDate")]
    [InlineData("STATUS")]
    [InlineData("CreatedAt")]
    public void Validate_WhenSortFieldIsValid_ShouldNotReturnSortFieldError(string sortField)
    {
        // Arrange
        var query = new GetEngagementsByClientQuery(1, 10)
        {
            ClientId = ClientId.Create(Guid.NewGuid()),
            SortField = sortField
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.DoesNotContain(result.ValidationFailureMessages, msg => msg.Contains("Sort field"));
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("clientname")]
    [InlineData("id")]
    [InlineData("description")]
    public void Validate_WhenSortFieldIsInvalid_ShouldReturnInvalidResponse(string sortField)
    {
        // Arrange
        var query = new GetEngagementsByClientQuery(1, 10)
        {
            ClientId = ClientId.Create(Guid.NewGuid()),
            SortField = sortField
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.ValidationFailureMessages, msg => msg.Contains("Sort field must be one of"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_WhenSortDirectionIsNullOrEmpty_ShouldNotReturnError(string? sortDirection)
    {
        // Arrange
        var query = new GetEngagementsByClientQuery(1, 10)
        {
            ClientId = ClientId.Create(Guid.NewGuid()),
            SortDirection = sortDirection
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.DoesNotContain(result.ValidationFailureMessages, msg => msg.Contains("Sort direction"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_WhenSortFieldIsNullOrEmpty_ShouldNotReturnError(string? sortField)
    {
        // Arrange
        var query = new GetEngagementsByClientQuery(1, 10)
        {
            ClientId = ClientId.Create(Guid.NewGuid()),
            SortField = sortField
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.DoesNotContain(result.ValidationFailureMessages, msg => msg.Contains("Sort field"));
    }

    [Fact]
    public void Validate_WhenSearchTermExceedsMaxLength_ShouldReturnInvalidResponse()
    {
        // Arrange
        var query = new GetEngagementsByClientQuery(1, 10)
        {
            ClientId = ClientId.Create(Guid.NewGuid()),
            SearchTerm = new string('a', 101)
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.ValidationFailureMessages, msg => msg.Contains("Search term cannot exceed 100 characters"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    [InlineData("Valid search term")]
    public void Validate_WhenSearchTermIsValidOrEmpty_ShouldNotReturnSearchTermError(string? searchTerm)
    {
        // Arrange
        var query = new GetEngagementsByClientQuery(1, 10)
        {
            ClientId = ClientId.Create(Guid.NewGuid()),
            SearchTerm = searchTerm
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.DoesNotContain(result.ValidationFailureMessages, msg => msg.Contains("Search term"));
    }

    [Fact]
    public void Validate_WhenSearchTermIsExactly100Characters_ShouldNotReturnError()
    {
        // Arrange
        var query = new GetEngagementsByClientQuery(1, 10)
        {
            ClientId = ClientId.Create(Guid.NewGuid()),
            SearchTerm = new string('a', 100)
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.DoesNotContain(result.ValidationFailureMessages, msg => msg.Contains("Search term"));
    }

    [Fact]
    public void Validate_WhenMultipleErrorsExist_ShouldReturnAllErrors()
    {
        // Arrange
        var query = new GetEngagementsByClientQuery(0, 0)
        {
            ClientId = ClientId.Create(Guid.NewGuid()),
            SortDirection = "invalid",
            SortField = "invalid",
            SearchTerm = new string('a', 101)
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.False(result.IsValid);
        var errorMessages = result.ValidationFailureMessages.ToList();
        Assert.Equal(5, errorMessages.Count);
        Assert.Contains(errorMessages, msg => msg.Contains("Page number must be greater than or equal to 1"));
        Assert.Contains(errorMessages, msg => msg.Contains("Page size must be greater than or equal to 1"));
        Assert.Contains(errorMessages, msg => msg.Contains("Sort direction must be either 'asc' or 'desc'"));
        Assert.Contains(errorMessages, msg => msg.Contains("Sort field must be one of"));
        Assert.Contains(errorMessages, msg => msg.Contains("Search term cannot exceed 100 characters"));
    }

    [Fact]
    public void Validate_WhenOptionalParametersAreNull_ShouldOnlyValidateRequiredFields()
    {
        // Arrange
        var query = new GetEngagementsByClientQuery(1, 10)
        {
            ClientId = ClientId.Create(Guid.NewGuid()),
            SearchTerm = null,
            SortField = null,
            SortDirection = null
        };

        // Act
        var result = query.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationFailureMessages);
    }
}

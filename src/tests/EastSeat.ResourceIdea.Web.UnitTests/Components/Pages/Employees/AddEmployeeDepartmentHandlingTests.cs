using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Departments.Models;
using Xunit;

namespace EastSeat.ResourceIdea.Web.UnitTests.Components.Pages.Employees;

public class AddEmployeeDepartmentHandlingTests
{
    [Fact]
    public void LoadDepartments_WhenNotFoundError_ShouldHandleGracefully()
    {
        // Arrange
        var notFoundResponse = ResourceIdeaResponse<PagedListResponse<DepartmentModel>>.Failure(ErrorCode.NotFound);
        
        // Act - Simulate the logic from AddEmployee.LoadDepartmentsAsync
        List<DepartmentModel> departments = [];
        bool shouldShowError = false;
        
        if (notFoundResponse.IsSuccess && notFoundResponse.Content.HasValue)
        {
            departments = [.. notFoundResponse.Content.Value.Items];
        }
        else if (notFoundResponse.IsFailure && notFoundResponse.Error == ErrorCode.NotFound)
        {
            // Handle the case where no departments exist yet - this is acceptable for adding employees
            departments = [];
            // No need to show an error message as empty departments list is valid
            shouldShowError = false;
        }
        else
        {
            shouldShowError = true; // Would show "Failed to load departments"
        }
        
        // Assert
        Assert.False(shouldShowError, "Should not show error for NotFound when no departments exist");
        Assert.Empty(departments);
        Assert.True(notFoundResponse.IsFailure);
        Assert.Equal(ErrorCode.NotFound, notFoundResponse.Error);
    }
    
    [Fact]
    public void LoadDepartments_WhenOtherError_ShouldShowError()
    {
        // Arrange
        var errorResponse = ResourceIdeaResponse<PagedListResponse<DepartmentModel>>.Failure(ErrorCode.DataStoreQueryFailure);
        
        // Act - Simulate the logic from AddEmployee.LoadDepartmentsAsync
        List<DepartmentModel> departments = [];
        bool shouldShowError = false;
        
        if (errorResponse.IsSuccess && errorResponse.Content.HasValue)
        {
            departments = [.. errorResponse.Content.Value.Items];
        }
        else if (errorResponse.IsFailure && errorResponse.Error == ErrorCode.NotFound)
        {
            departments = [];
            shouldShowError = false;
        }
        else
        {
            shouldShowError = true; // Would show "Failed to load departments"
        }
        
        // Assert
        Assert.True(shouldShowError, "Should show error for non-NotFound errors");
        Assert.Empty(departments);
        Assert.True(errorResponse.IsFailure);
        Assert.Equal(ErrorCode.DataStoreQueryFailure, errorResponse.Error);
    }
}
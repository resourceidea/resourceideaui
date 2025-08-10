using EastSeat.ResourceIdea.Web.Components.Pages.Employees;
using Xunit;

namespace EastSeat.ResourceIdea.Web.UnitTests.Components.Pages.Employees;

public class AddEmployeeNavigationTests
{
    [Fact]
    public void GetReturnUrl_WhenJobPositionDetailsReturnView_ShouldReturnCorrectUrl()
    {
        // Arrange
        var component = new AddEmployee();
        var testGuid = Guid.NewGuid();
        
        // Use reflection to set the parameters since they're normally set by the framework
        var returnViewProperty = typeof(AddEmployee).GetProperty("ReturnView");
        var returnIdProperty = typeof(AddEmployee).GetProperty("ReturnId");
        
        returnViewProperty?.SetValue(component, "jobposition-details");
        returnIdProperty?.SetValue(component, testGuid.ToString());
        
        // Act
        var returnUrl = component.GetType()
            .GetMethod("GetReturnUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(component, null) as string;
        
        // Assert
        Assert.Equal($"/jobpositions/{testGuid}", returnUrl);
    }
    
    [Fact]
    public void GetReturnUrl_WhenDepartmentDetailsReturnView_ShouldReturnCorrectUrl()
    {
        // Arrange
        var component = new AddEmployee();
        var testGuid = Guid.NewGuid();
        
        // Use reflection to set the parameters
        var returnViewProperty = typeof(AddEmployee).GetProperty("ReturnView");
        var returnIdProperty = typeof(AddEmployee).GetProperty("ReturnId");
        
        returnViewProperty?.SetValue(component, "department-details");
        returnIdProperty?.SetValue(component, testGuid.ToString());
        
        // Act
        var returnUrl = component.GetType()
            .GetMethod("GetReturnUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(component, null) as string;
        
        // Assert
        Assert.Equal($"/departments/{testGuid}", returnUrl);
    }
    
    [Fact]
    public void GetReturnUrl_WhenNoReturnViewProvided_ShouldReturnDefaultUrl()
    {
        // Arrange
        var component = new AddEmployee();
        
        // Act - Don't set ReturnView and ReturnId (they remain null/empty)
        var returnUrl = component.GetType()
            .GetMethod("GetReturnUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(component, null) as string;
        
        // Assert
        Assert.Equal("/employees", returnUrl);
    }
    
    [Fact]
    public void GetReturnUrl_WhenUnknownReturnView_ShouldReturnDefaultUrl()
    {
        // Arrange
        var component = new AddEmployee();
        var testGuid = Guid.NewGuid();
        
        // Use reflection to set the parameters
        var returnViewProperty = typeof(AddEmployee).GetProperty("ReturnView");
        var returnIdProperty = typeof(AddEmployee).GetProperty("ReturnId");
        
        returnViewProperty?.SetValue(component, "unknown-view");
        returnIdProperty?.SetValue(component, testGuid.ToString());
        
        // Act
        var returnUrl = component.GetType()
            .GetMethod("GetReturnUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(component, null) as string;
        
        // Assert
        Assert.Equal("/employees", returnUrl);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GetReturnUrl_WhenEmptyOrNullReturnId_ShouldReturnDefaultUrl(string? returnId)
    {
        // Arrange
        var component = new AddEmployee();
        
        // Use reflection to set the parameters
        var returnViewProperty = typeof(AddEmployee).GetProperty("ReturnView");
        var returnIdProperty = typeof(AddEmployee).GetProperty("ReturnId");
        
        returnViewProperty?.SetValue(component, "jobposition-details");
        returnIdProperty?.SetValue(component, returnId);
        
        // Act
        var returnUrl = component.GetType()
            .GetMethod("GetReturnUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(component, null) as string;
        
        // Assert
        Assert.Equal("/employees", returnUrl);
    }
}
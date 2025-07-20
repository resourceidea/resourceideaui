// -------------------------------------------------------------------------------
// File: BackendDevToolsTests.cs
// Path: src/tests/EastSeat.ResourceIdea.Web.UnitTests/Components/Pages/Backend/BackendDevToolsTests.cs
// Description: Unit tests for backend DevTools page authorization and functionality
// -------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Web.Authorization;
using EastSeat.ResourceIdea.Web.Components.Pages.Backend;
using Microsoft.AspNetCore.Authorization;
using Xunit;
using System.Reflection;

namespace EastSeat.ResourceIdea.Web.UnitTests.Components.Pages.Backend;

public class BackendDevToolsTests
{
    [Fact]
    public void BackendDevTools_ShouldHaveBackendAccessAttribute()
    {
        // Arrange & Act
        var type = typeof(DevTools);
        var attributes = type.GetCustomAttributes(typeof(BackendAccessAttribute), false);

        // Assert
        Assert.Single(attributes);
        Assert.IsType<BackendAccessAttribute>(attributes[0]);
    }

    [Fact]
    public void BackendDevTools_ShouldNotHaveAllowAnonymousAttribute()
    {
        // Arrange & Act
        var type = typeof(DevTools);
        var attributes = type.GetCustomAttributes(typeof(AllowAnonymousAttribute), false);

        // Assert
        Assert.Empty(attributes);
    }

    [Fact]
    public void BackendDevTools_ShouldInheritFromResourceIdeaComponentBase()
    {
        // Arrange & Act
        var type = typeof(DevTools);
        var baseType = type.BaseType;

        // Assert
        Assert.NotNull(baseType);
        Assert.Equal("ResourceIdeaComponentBase", baseType.Name);
    }

    [Fact]
    public void BackendAccessAttribute_ShouldSetCorrectPolicy()
    {
        // Arrange & Act
        var attribute = new BackendAccessAttribute();

        // Assert
        Assert.Equal("BackendAccess", attribute.Policy);
    }
}
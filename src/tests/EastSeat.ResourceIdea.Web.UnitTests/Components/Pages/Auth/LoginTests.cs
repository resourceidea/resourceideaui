using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Users.Models;
using EastSeat.ResourceIdea.Web.Components.Pages.Auth;
using EastSeat.ResourceIdea.Web.Services;
using EastSeat.ResourceIdea.Web.UnitTests.TestHelpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EastSeat.ResourceIdea.Web.UnitTests.Components.Pages.Auth;

/// <summary>
/// Unit tests for the Login component.
/// </summary>
public class LoginTests
{
    private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly TestNavigationManager _testNavigationManager;
    private readonly Mock<IExceptionHandlingService> _mockExceptionHandlingService; public LoginTests()
    {
        // Setup UserManager mock
        var userStore = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            userStore.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        // Setup SignInManager mock
        var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
        var logger = new Mock<ILogger<SignInManager<ApplicationUser>>>();

        _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
            _mockUserManager.Object,
            contextAccessor.Object,
            claimsFactory.Object,
            null!,
            logger.Object,
            null!,
            null!);

        // Setup TestNavigationManager
        _testNavigationManager = new TestNavigationManager();

        // Setup ExceptionHandlingService mock
        _mockExceptionHandlingService = new Mock<IExceptionHandlingService>();
    }

    [Fact]
    public async Task HandleLoginWithLoadingState_WithValidCredentials_ShouldNavigateToDefaultPage()
    {
        // Arrange
        _mockSignInManager
            .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.Success);

        var login = new Login();
        SetPrivateProperty(login, "SignInManager", _mockSignInManager.Object);
        SetPrivateProperty(login, "Navigation", _testNavigationManager);
        SetPrivateProperty(login, "ExceptionHandlingService", _mockExceptionHandlingService.Object);
        SetPrivateField(login, "loginModel", new LoginModel { Email = "test@example.com", Password = "password123" });

        // Act
        var handleLoginMethod = typeof(Login).GetMethod("HandleLoginWithLoadingState",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var task = (Task?)handleLoginMethod?.Invoke(login, null);
        if (task != null) await task;

        // Assert
        _testNavigationManager.VerifyNavigatedTo("/", true);
        _testNavigationManager.VerifyNavigationCallCount(1);
    }

    [Fact]
    public async Task HandleLoginWithLoadingState_WithReturnUrl_ShouldNavigateToReturnUrl()
    {
        // Arrange
        _mockSignInManager
            .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.Success);

        var login = new Login();
        SetPrivateProperty(login, "SignInManager", _mockSignInManager.Object);
        SetPrivateProperty(login, "Navigation", _testNavigationManager);
        SetPrivateProperty(login, "ExceptionHandlingService", _mockExceptionHandlingService.Object);
        SetPrivateProperty(login, "ReturnUrl", "/custom-page");
        SetPrivateField(login, "loginModel", new LoginModel { Email = "test@example.com", Password = "password123" });

        // Act
        var handleLoginMethod = typeof(Login).GetMethod("HandleLoginWithLoadingState",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var task = (Task?)handleLoginMethod?.Invoke(login, null);
        if (task != null) await task;

        // Assert
        _testNavigationManager.VerifyNavigatedTo("/custom-page", true);
        _testNavigationManager.VerifyNavigationCallCount(1);
    }

    [Fact]
    public async Task HandleLoginWithLoadingState_WithInvalidCredentials_ShouldSetErrorMessage()
    {
        // Arrange
        _mockSignInManager
            .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.Failed);

        var login = new Login();
        SetPrivateProperty(login, "SignInManager", _mockSignInManager.Object);
        SetPrivateProperty(login, "Navigation", _testNavigationManager);
        SetPrivateProperty(login, "ExceptionHandlingService", _mockExceptionHandlingService.Object);
        SetPrivateField(login, "loginModel", new LoginModel { Email = "test@example.com", Password = "wrongpassword" });

        // Act
        var handleLoginMethod = typeof(Login).GetMethod("HandleLoginWithLoadingState",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var task = (Task?)handleLoginMethod?.Invoke(login, null);
        if (task != null) await task;

        // Assert
        var hasError = GetPrivateProperty<bool>(login, "HasError");
        var errorMessage = GetPrivateProperty<string>(login, "ErrorMessage");
        Assert.True(hasError);
        Assert.Equal("Invalid email or password.", errorMessage);

        // Verify no navigation occurred
        _testNavigationManager.VerifyNavigationCallCount(0);
    }

    [Fact]
    public async Task HandleLoginWithLoadingState_WhenException_ShouldSetGenericErrorMessage()
    {
        // Arrange
        _mockSignInManager
            .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ThrowsAsync(new InvalidOperationException("Test exception"));

        _mockExceptionHandlingService
            .Setup(x => x.HandleExceptionAsync(It.IsAny<Exception>(), It.IsAny<ComponentBase>(), It.IsAny<string>()))
            .ReturnsAsync("An error occurred during login. Please try again.");

        var login = new Login();
        SetPrivateProperty(login, "SignInManager", _mockSignInManager.Object);
        SetPrivateProperty(login, "Navigation", _testNavigationManager);
        SetPrivateProperty(login, "ExceptionHandlingService", _mockExceptionHandlingService.Object);
        SetPrivateField(login, "loginModel", new LoginModel { Email = "test@example.com", Password = "password123" });

        // Act
        var handleLoginMethod = typeof(Login).GetMethod("HandleLoginWithLoadingState",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var task = (Task?)handleLoginMethod?.Invoke(login, null);
        if (task != null) await task;

        // Assert
        var hasError = GetPrivateProperty<bool>(login, "HasError");
        var errorMessage = GetPrivateProperty<string>(login, "ErrorMessage");
        Assert.True(hasError);
        Assert.Equal("An error occurred during login. Please try again.", errorMessage);

        // Verify no navigation occurred
        _testNavigationManager.VerifyNavigationCallCount(0);
    }
    [Fact]
    public void GetDisplayErrorMessage_WithErrorMessage_ShouldReturnErrorMessage()
    {
        // Arrange
        var login = new Login();
        SetPrivateProperty(login, "ErrorMessage", "Test error message");

        // Act
        var method = typeof(Login).GetMethod("GetDisplayErrorMessage",
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var result = method?.Invoke(login, null) as string;

        // Assert
        Assert.Equal("Test error message", result);
    }

    [Fact]
    public void GetDisplayErrorMessage_WithoutErrorMessage_ShouldReturnEmptyString()
    {
        // Arrange
        var login = new Login();
        // ErrorMessage is null by default, no need to set it

        // Act
        var method = typeof(Login).GetMethod("GetDisplayErrorMessage",
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var result = method?.Invoke(login, null) as string;

        // Assert
        Assert.Equal(string.Empty, result);
    }

    private static void SetPrivateProperty(object obj, string propertyName, object value)
    {
        var property = obj.GetType().GetProperty(propertyName,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        property?.SetValue(obj, value);
    }

    private static void SetPrivateField(object obj, string fieldName, object value)
    {
        var field = obj.GetType().GetField(fieldName,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field?.SetValue(obj, value);
    }

    private static T GetPrivateField<T>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (T)(field?.GetValue(obj) ?? default(T)!);
    }

    private static T GetPrivateProperty<T>(object obj, string propertyName)
    {
        var property = obj.GetType().GetProperty(propertyName,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        return (T)(property?.GetValue(obj) ?? default(T)!);
    }
}

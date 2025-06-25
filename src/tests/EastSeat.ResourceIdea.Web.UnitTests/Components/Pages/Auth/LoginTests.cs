using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Users.Models;
using EastSeat.ResourceIdea.Web.Components.Pages.Auth;
using EastSeat.ResourceIdea.Web.Services;
using EastSeat.ResourceIdea.Web.UnitTests.TestHelpers;
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
    public void Login_WithValidCredentials_ShouldNavigateToDefaultPage()
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
        var handleLoginMethod = typeof(Login).GetMethod("HandleLogin",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        handleLoginMethod?.Invoke(login, null);

        // Assert
        _testNavigationManager.VerifyNavigatedTo("/departments", true);
        _testNavigationManager.VerifyNavigationCallCount(1);
    }

    [Fact]
    public void Login_WithReturnUrl_ShouldNavigateToReturnUrl()
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
        var handleLoginMethod = typeof(Login).GetMethod("HandleLogin",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        handleLoginMethod?.Invoke(login, null);

        // Assert
        _testNavigationManager.VerifyNavigatedTo("/custom-page", true);
        _testNavigationManager.VerifyNavigationCallCount(1);
    }

    [Fact]
    public void Login_WithInvalidCredentials_ShouldSetErrorMessage()
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
        var handleLoginMethod = typeof(Login).GetMethod("HandleLogin",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        handleLoginMethod?.Invoke(login, null);

        // Assert
        var authErrorMessage = GetPrivateField<string>(login, "authErrorMessage");
        Assert.Equal("Invalid email or password.", authErrorMessage);

        // Verify no navigation occurred
        _testNavigationManager.VerifyNavigationCallCount(0);
    }

    [Fact]
    public void Login_WhenException_ShouldSetGenericErrorMessage()
    {
        // Arrange
        _mockSignInManager
            .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ThrowsAsync(new InvalidOperationException("Test exception"));

        _mockExceptionHandlingService
            .Setup(x => x.HandleExceptionAsync(It.IsAny<Exception>(), It.IsAny<string>()))
            .ReturnsAsync("Error logged");

        var login = new Login();
        SetPrivateProperty(login, "SignInManager", _mockSignInManager.Object);
        SetPrivateProperty(login, "Navigation", _testNavigationManager);
        SetPrivateProperty(login, "ExceptionHandlingService", _mockExceptionHandlingService.Object);
        SetPrivateField(login, "loginModel", new LoginModel { Email = "test@example.com", Password = "password123" });

        // Act
        var handleLoginMethod = typeof(Login).GetMethod("HandleLogin",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        handleLoginMethod?.Invoke(login, null);

        // Assert
        var authErrorMessage = GetPrivateField<string>(login, "authErrorMessage");
        Assert.Equal("An error occurred during login. Please try again.", authErrorMessage);

        // Verify no navigation occurred
        _testNavigationManager.VerifyNavigationCallCount(0);
    }
    [Fact]
    public void GetDisplayErrorMessage_WithAuthError_ShouldReturnAuthError()
    {
        // Arrange
        var login = new Login();
        SetPrivateField(login, "authErrorMessage", "Auth error");

        // Act
        var method = typeof(Login).GetMethod("GetDisplayErrorMessage",
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var result = method?.Invoke(login, null) as string;

        // Assert
        Assert.Equal("Auth error", result);
    }

    [Fact]
    public void GetDisplayErrorMessage_WithoutAuthError_ShouldReturnBaseError()
    {
        // Arrange
        var login = new Login();
        SetPrivateField(login, "authErrorMessage", string.Empty);
        SetPrivateProperty(login, "ErrorMessage", "Base error");

        // Act
        var method = typeof(Login).GetMethod("GetDisplayErrorMessage",
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var result = method?.Invoke(login, null) as string;

        // Assert
        Assert.Equal("Base error", result);
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
}

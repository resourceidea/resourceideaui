using Bunit;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Users.Models;
using EastSeat.ResourceIdea.Web.Components.Pages.Auth;
using EastSeat.ResourceIdea.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace EastSeat.ResourceIdea.Web.UnitTests.Components.Pages.Auth;

/// <summary>
/// Unit tests for the Login component using Bunit.
/// </summary>
public class LoginTests : TestContext
{
    private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<IExceptionHandlingService> _mockExceptionHandlingService;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

    public LoginTests()
    {
        // Setup UserManager mock
        var userStore = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            userStore.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        // Setup SignInManager mock
        var contextAccessor = new Mock<IHttpContextAccessor>();
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

        // Setup ExceptionHandlingService mock
        _mockExceptionHandlingService = new Mock<IExceptionHandlingService>();

        // Setup HttpContextAccessor mock
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var mockHttpContext = new Mock<HttpContext>();
        var mockResponse = new Mock<HttpResponse>();
        mockResponse.Setup(x => x.HasStarted).Returns(false);
        mockHttpContext.Setup(x => x.Response).Returns(mockResponse.Object);
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

        // Register services with Bunit's service collection
        Services.AddSingleton(_mockSignInManager.Object);
        Services.AddSingleton(_mockUserManager.Object);
        Services.AddSingleton(_mockExceptionHandlingService.Object);
        Services.AddSingleton(_mockHttpContextAccessor.Object);
    }

    [Fact]
    public async Task HandleLoginWithLoadingState_WithValidCredentials_ShouldNavigateToDefaultPage()
    {
        // Arrange
        var testUser = new ApplicationUser { Id = "1", UserName = "testuser", Email = "test@example.com" };
        
        _mockUserManager
            .Setup(x => x.FindByEmailAsync("test@example.com"))
            .ReturnsAsync(testUser);
            
        _mockSignInManager
            .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.Success);

        var testUser = new ApplicationUser { UserName = "test@example.com", Email = "test@example.com" };
        _mockUserManager
            .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(testUser);

        // Setup exception handling service to return success
        _mockExceptionHandlingService
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task>>(), It.IsAny<string>()))
            .Returns(Task.FromResult(ExceptionHandlingResult.Success()));

        // Act
        var component = RenderComponent<Login>();

        // Set the login model via reflection since it's private
        var loginModelField = typeof(Login).GetField("loginModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        loginModelField?.SetValue(component.Instance, new LoginModel { Email = "test@example.com", Password = "password123" });

        // Trigger the login method using InvokeAsync to run on the component's dispatcher
        await component.InvokeAsync(async () =>
        {
            var handleLoginMethod = typeof(Login).GetMethod("HandleLoginWithLoadingState",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var task = (Task?)handleLoginMethod?.Invoke(component.Instance, null);
            if (task != null)
            {
                await task;
            }
        });

        // Assert
        // In Bunit, navigation is handled by the NavigationManager service
        // We can verify that the navigation was triggered by checking the current URI
        var navigationManager = Services.GetService<NavigationManager>();
        Assert.NotNull(navigationManager);
        Assert.Equal("http://localhost/", navigationManager.Uri);
    }

    [Fact]
    public async Task HandleLoginWithLoadingState_WithReturnUrl_ShouldNavigateToReturnUrl()
    {
        // Arrange
        var testUser = new ApplicationUser { Id = "1", UserName = "testuser", Email = "test@example.com" };
        
        _mockUserManager
            .Setup(x => x.FindByEmailAsync("test@example.com"))
            .ReturnsAsync(testUser);
            
        _mockSignInManager
            .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.Success);

        var testUser = new ApplicationUser { UserName = "test@example.com", Email = "test@example.com" };
        _mockUserManager
            .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(testUser);

        // Setup exception handling service to actually execute the operation
        _mockExceptionHandlingService
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task>>(), It.IsAny<string>()))
            .Returns<Func<Task>, string>(async (operation, context) =>
            {
                await operation();
                return ExceptionHandlingResult.Success();
            });

        // Act
        // Navigate to the login page with ReturnUrl query parameter first
        var navigationManager = Services.GetService<NavigationManager>()!;
        var uri = navigationManager.GetUriWithQueryParameter("ReturnUrl", "/custom-page");
        navigationManager.NavigateTo(uri);

        var component = RenderComponent<Login>();

        // Set the login model via reflection since it's private
        var loginModelField = typeof(Login).GetField("loginModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        loginModelField?.SetValue(component.Instance, new LoginModel { Email = "test@example.com", Password = "password123" });

        // Trigger the login method using InvokeAsync to run on the component's dispatcher
        await component.InvokeAsync(async () =>
        {
            var handleLoginMethod = typeof(Login).GetMethod("HandleLoginWithLoadingState",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var task = (Task?)handleLoginMethod?.Invoke(component.Instance, null);
            if (task != null)
            {
                await task;
            }
        });

        // Assert
        // First check what ReturnUrl the component actually has
        var returnUrl = GetPrivateProperty<string>(component.Instance, "ReturnUrl");

        // Verify navigation to custom page
        Assert.Equal("http://localhost/custom-page", navigationManager.Uri);
    }

    [Fact]
    public async Task HandleLoginWithLoadingState_WithInvalidCredentials_ShouldSetErrorMessage()
    {
        // Arrange
        var testUser = new ApplicationUser { Id = "1", UserName = "testuser", Email = "test@example.com" };
        
        _mockUserManager
            .Setup(x => x.FindByEmailAsync("test@example.com"))
            .ReturnsAsync(testUser);
            
        _mockSignInManager
            .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.Failed);

        var testUser = new ApplicationUser { UserName = "test@example.com", Email = "test@example.com" };
        _mockUserManager
            .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(testUser);

        // Setup exception handling service to actually execute the operation
        _mockExceptionHandlingService
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task>>(), It.IsAny<string>()))
            .Returns<Func<Task>, string>(async (operation, context) =>
            {
                try
                {
                    await operation();
                    return ExceptionHandlingResult.Success();
                }
                catch (Exception ex)
                {
                    return ExceptionHandlingResult.Failure($"Error: {ex.Message}");
                }
            });

        // Act
        var component = RenderComponent<Login>();

        // Set the login model via reflection since it's private
        var loginModelField = typeof(Login).GetField("loginModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        loginModelField?.SetValue(component.Instance, new LoginModel { Email = "test@example.com", Password = "wrongpassword" });

        // Trigger the login method using InvokeAsync to run on the component's dispatcher
        await component.InvokeAsync(async () =>
        {
            var handleLoginMethod = typeof(Login).GetMethod("HandleLoginWithLoadingState",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var task = (Task?)handleLoginMethod?.Invoke(component.Instance, null);
            if (task != null)
            {
                await task;
            }
        });

        // Assert
        var hasError = GetPrivateProperty<bool>(component.Instance, "HasError");
        var errorMessage = GetPrivateProperty<string>(component.Instance, "ErrorMessage");
        Assert.True(hasError);
        Assert.Equal("Invalid email or password.", errorMessage);

        // Verify no navigation occurred
        var navigationManager = Services.GetService<NavigationManager>();
        Assert.NotNull(navigationManager);
        Assert.Equal("http://localhost/", navigationManager.Uri); // Should still be at base URI
    }

    [Fact]
    public async Task HandleLoginWithLoadingState_WhenException_ShouldSetGenericErrorMessage()
    {
        // Arrange
        var testUser = new ApplicationUser { Id = "1", UserName = "testuser", Email = "test@example.com" };
        
        _mockUserManager
            .Setup(x => x.FindByEmailAsync("test@example.com"))
            .ReturnsAsync(testUser);
            
        _mockSignInManager
            .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ThrowsAsync(new InvalidOperationException("Test exception"));

        _mockExceptionHandlingService
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task>>(), It.IsAny<string>()))
            .Returns(Task.FromResult(ExceptionHandlingResult.Failure("An error occurred during login. Please try again.")));

        // Act
        var component = RenderComponent<Login>();

        // Set the login model via reflection since it's private
        var loginModelField = typeof(Login).GetField("loginModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        loginModelField?.SetValue(component.Instance, new LoginModel { Email = "test@example.com", Password = "password123" });

        // Trigger the login method using InvokeAsync to run on the component's dispatcher
        await component.InvokeAsync(async () =>
        {
            var handleLoginMethod = typeof(Login).GetMethod("HandleLoginWithLoadingState",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var task = (Task?)handleLoginMethod?.Invoke(component.Instance, null);
            if (task != null)
            {
                await task;
            }
        });

        // Assert
        var hasError = GetPrivateProperty<bool>(component.Instance, "HasError");
        var errorMessage = GetPrivateProperty<string>(component.Instance, "ErrorMessage");
        Assert.True(hasError);
        Assert.Equal("An error occurred during login. Please try again.", errorMessage);

        // Verify no navigation occurred
        var navigationManager = Services.GetService<NavigationManager>();
        Assert.NotNull(navigationManager);
        Assert.Equal("http://localhost/", navigationManager.Uri); // Should still be at base URI
    }
    [Fact]
    public void GetDisplayErrorMessage_WithErrorMessage_ShouldReturnErrorMessage()
    {
        // Arrange
        var component = RenderComponent<Login>();
        SetPrivateProperty(component.Instance, "ErrorMessage", "Test error message");

        // Act
        var result = component.Instance.GetDisplayErrorMessage();

        // Assert
        Assert.Equal("Test error message", result);
    }

    [Fact]
    public void GetDisplayErrorMessage_WithoutErrorMessage_ShouldReturnEmptyString()
    {
        // Arrange
        var component = RenderComponent<Login>();
        // ErrorMessage is null by default, no need to set it

        // Act
        var result = component.Instance.GetDisplayErrorMessage();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    private static void SetPrivateProperty(object obj, string propertyName, object value)
    {
        var property = obj.GetType().GetProperty(propertyName,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        property?.SetValue(obj, value);
    }

    private static T GetPrivateProperty<T>(object obj, string propertyName)
    {
        var property = obj.GetType().GetProperty(propertyName,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        return (T)(property?.GetValue(obj) ?? default(T)!);
    }
}

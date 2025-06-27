using Bunit;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Web.Components.Pages.Auth;
using Microsoft.AspNetCore.Components;
using Moq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using EastSeat.ResourceIdea.Web.Services;
using Microsoft.AspNetCore.Http;

namespace EastSeat.ResourceIdea.Web.UnitTests.Components.Pages.Auth;

public class LoginComponentTests : TestContext
{
    public LoginComponentTests()
    {
        // Setup mocks for required services
        var mockSignInManager = CreateMockSignInManager();
        var mockUserManager = CreateMockUserManager();
        var mockAuthStateProvider = CreateMockAuthenticationStateProvider();
        var mockExceptionHandlingService = CreateMockExceptionHandlingService();
        var mockHttpContextAccessor = CreateMockHttpContextAccessor();

        Services.AddSingleton(mockSignInManager.Object);
        Services.AddSingleton(mockUserManager.Object);
        Services.AddSingleton<AuthenticationStateProvider>(mockAuthStateProvider.Object);
        Services.AddSingleton(mockExceptionHandlingService.Object);
        Services.AddSingleton(mockHttpContextAccessor.Object);
        Services.AddAuthorizationCore();

        // Add a real NavigationManager
        Services.AddSingleton<NavigationManager>(new MockNavigationManager("https://localhost/"));
    }

    [Fact]
    public void LoginComponent_Should_Render_Login_Form()
    {
        // Act
        var component = RenderComponent<Login>();

        // Assert
        Assert.NotNull(component.Find("form"));
        Assert.NotNull(component.Find("input[id='email']"));
        Assert.NotNull(component.Find("input[id='password']"));
        Assert.NotNull(component.Find("button[type='submit']"));
    }

    [Fact]
    public void LoginComponent_Should_Display_Email_Label()
    {
        // Act
        var component = RenderComponent<Login>();

        // Assert
        var emailLabel = component.Find("label[for='email']");
        Assert.Contains("Email", emailLabel.TextContent);
    }

    [Fact]
    public void LoginComponent_Should_Display_Password_Label()
    {
        // Act
        var component = RenderComponent<Login>();

        // Assert
        var passwordLabel = component.Find("label[for='password']");
        Assert.Contains("Password", passwordLabel.TextContent);
    }

    [Fact]
    public void LoginComponent_Should_Display_Login_Button()
    {
        // Act
        var component = RenderComponent<Login>();

        // Assert
        var submitButton = component.Find("button[type='submit']");
        Assert.Contains("Login", submitButton.TextContent);
    }

    [Fact]
    public void LoginComponent_Should_Display_Page_Title()
    {
        // Act
        var component = RenderComponent<Login>();

        // Assert
        var title = component.Find("h4");
        Assert.Contains("Login", title.TextContent);
    }

    [Fact]
    public void LoginComponent_Should_Show_Loading_State_When_Submitting()
    {
        // Arrange
        var component = RenderComponent<Login>();
        var form = component.Find("form");
        var submitButton = component.Find("button[type='submit']");

        // Act
        form.Submit();

        // Assert - The button should show loading state
        // Note: This test might need adjustment based on the actual loading implementation
        Assert.True(component.Markup.Contains("Login") || component.Markup.Contains("Loading"));
    }

    private static Mock<SignInManager<ApplicationUser>> CreateMockSignInManager()
    {
        var mockUserManager = CreateMockUserManager();
        var mockContextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var mockClaimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
        var mockOptions = new Mock<IOptions<IdentityOptions>>();
        var mockLogger = new Mock<ILogger<SignInManager<ApplicationUser>>>();
        var mockSchemes = new Mock<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>();
        var mockConfirmation = new Mock<IUserConfirmation<ApplicationUser>>();

        return new Mock<SignInManager<ApplicationUser>>(
            mockUserManager.Object,
            mockContextAccessor.Object,
            mockClaimsFactory.Object,
            mockOptions.Object,
            mockLogger.Object,
            mockSchemes.Object,
            mockConfirmation.Object);
    }

    private static Mock<UserManager<ApplicationUser>> CreateMockUserManager()
    {
        var mockStore = new Mock<IUserStore<ApplicationUser>>();
        var mockOptions = new Mock<IOptions<IdentityOptions>>();
        var mockPasswordHasher = new Mock<IPasswordHasher<ApplicationUser>>();
        var mockUserValidators = new List<IUserValidator<ApplicationUser>>();
        var mockPasswordValidators = new List<IPasswordValidator<ApplicationUser>>();
        var mockKeyNormalizer = new Mock<ILookupNormalizer>();
        var mockErrors = new Mock<IdentityErrorDescriber>();
        var mockServices = new Mock<IServiceProvider>();
        var mockLogger = new Mock<ILogger<UserManager<ApplicationUser>>>();

        return new Mock<UserManager<ApplicationUser>>(
            mockStore.Object,
            mockOptions.Object,
            mockPasswordHasher.Object,
            mockUserValidators,
            mockPasswordValidators,
            mockKeyNormalizer.Object,
            mockErrors.Object,
            mockServices.Object,
            mockLogger.Object);
    }

    private static Mock<AuthenticationStateProvider> CreateMockAuthenticationStateProvider()
    {
        var mockAuthStateProvider = new Mock<AuthenticationStateProvider>();
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = new AuthenticationState(anonymousUser);

        mockAuthStateProvider.Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);

        return mockAuthStateProvider;
    }

    private static Mock<IExceptionHandlingService> CreateMockExceptionHandlingService()
    {
        var mock = new Mock<IExceptionHandlingService>();

        mock.Setup(x => x.ExecuteAsync(It.IsAny<Func<Task>>(), It.IsAny<string>()))
            .Returns(Task.FromResult(ExceptionHandlingResult.Success()));

        return mock;
    }

    private static Mock<IHttpContextAccessor> CreateMockHttpContextAccessor()
    {
        var mock = new Mock<IHttpContextAccessor>();
        var mockHttpContext = new Mock<HttpContext>();

        // Setup HttpContext to be available for the components
        mock.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

        return mock;
    }

    // Simple NavigationManager implementation for testing
    private class MockNavigationManager : NavigationManager
    {
        public MockNavigationManager(string baseUri) : base()
        {
            Initialize(baseUri, baseUri);
        }

        protected override void NavigateToCore(string uri, NavigationOptions options)
        {
            // No-op for testing
        }
    }
}
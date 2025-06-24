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

namespace EastSeat.ResourceIdea.Web.UnitTests.Components.Pages.Auth;

public class LogoutComponentTests : TestContext
{
    public LogoutComponentTests()
    {
        // Setup mocks for required services
        var mockSignInManager = CreateMockSignInManager();
        var mockAuthStateProvider = CreateMockAuthenticationStateProvider();
        var mockExceptionHandlingService = CreateMockExceptionHandlingService();

        Services.AddSingleton(mockSignInManager.Object);
        Services.AddSingleton<AuthenticationStateProvider>(mockAuthStateProvider.Object);
        Services.AddSingleton(mockExceptionHandlingService.Object);
        Services.AddAuthorizationCore();

        // Add a real NavigationManager
        Services.AddSingleton<NavigationManager>(new MockNavigationManager("https://localhost/"));
    }

    [Fact]
    public void LogoutComponent_Should_Render_Without_Errors()
    {
        // Act & Assert - Should not throw exceptions
        var component = RenderComponent<Logout>();
        Assert.NotNull(component);
    }

    [Fact]
    public void LogoutComponent_Should_Call_SignOut_On_Initialization()
    {
        // Arrange
        var mockSignInManager = CreateMockSignInManager();
        var mockExceptionHandlingService = CreateMockExceptionHandlingService();
        
        Services.AddSingleton(mockSignInManager.Object);
        Services.AddSingleton(mockExceptionHandlingService.Object);
        Services.AddSingleton<NavigationManager>(new MockNavigationManager("https://localhost/"));

        // Act
        var component = RenderComponent<Logout>();

        // Assert
        mockSignInManager.Verify(x => x.SignOutAsync(), Times.Once);
    }

    [Fact]
    public void LogoutComponent_Should_Navigate_To_Home_After_SignOut()
    {
        // Arrange
        var mockSignInManager = CreateMockSignInManager();
        var mockExceptionHandlingService = CreateMockExceptionHandlingService();
        var mockNavigationManager = new MockNavigationManager("https://localhost/");
        
        Services.AddSingleton(mockSignInManager.Object);
        Services.AddSingleton(mockExceptionHandlingService.Object);
        Services.AddSingleton<NavigationManager>(mockNavigationManager);

        // Act
        var component = RenderComponent<Logout>();

        // Assert - Check that navigation was called (this will be called during the OnInitializedAsync)
        Assert.True(mockNavigationManager.NavigationCalled);
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

        var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
            mockUserManager.Object,
            mockContextAccessor.Object,
            mockClaimsFactory.Object,
            mockOptions.Object,
            mockLogger.Object,
            mockSchemes.Object,
            mockConfirmation.Object);

        mockSignInManager.Setup(x => x.SignOutAsync())
            .Returns(Task.CompletedTask);

        return mockSignInManager;
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

    // Simple NavigationManager implementation for testing
    private class MockNavigationManager : NavigationManager
    {
        public bool NavigationCalled { get; private set; }

        public MockNavigationManager(string baseUri) : base()
        {
            Initialize(baseUri, baseUri);
        }

        protected override void NavigateToCore(string uri, NavigationOptions options)
        {
            NavigationCalled = true;
        }
    }
}
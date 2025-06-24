# Authentication Testing with BUnit and Playwright

This document describes the comprehensive testing strategy implemented for the authentication functionality in the ResourceIdea application.

## Testing Structure

### 1. BUnit Component Tests (`EastSeat.ResourceIdea.Web.UnitTests`)

BUnit tests focus on testing individual Blazor components in isolation:

#### Login Component Tests (`LoginComponentTests.cs`)
- **LoginComponent_Should_Render_Login_Form**: Verifies that the login form renders with all required elements
- **LoginComponent_Should_Display_Email_Label**: Tests that the email field label is displayed correctly
- **LoginComponent_Should_Display_Password_Label**: Tests that the password field label is displayed correctly
- **LoginComponent_Should_Display_Login_Button**: Verifies the submit button is rendered with correct text
- **LoginComponent_Should_Display_Page_Title**: Tests that the page title is rendered correctly
- **LoginComponent_Should_Show_Loading_State_When_Submitting**: Tests the loading state behavior during form submission

#### Logout Component Tests (`LogoutComponentTests.cs`)
- **LogoutComponent_Should_Render_Without_Errors**: Verifies the component renders without throwing exceptions
- **LogoutComponent_Should_Call_SignOut_On_Initialization**: Tests that SignOutAsync is called when component initializes
- **LogoutComponent_Should_Navigate_To_Home_After_SignOut**: Verifies navigation to home page after logout

### 2. End-to-End (E2E) Tests (`EastSeat.ResourceIdea.Web.E2ETests`)

E2E tests verify the complete authentication workflow from a user perspective:

#### Authentication E2E Tests (`AuthenticationE2ETests.cs`)
- **Login_Page_Should_Be_Accessible_Without_Authentication**: Verifies unauthenticated users can access login page
- **Home_Page_Should_Be_Accessible_Without_Authentication**: Tests that home page is publicly accessible
- **Protected_Page_Should_Redirect_To_Login_When_Not_Authenticated**: Verifies protected pages redirect to login
- **Protected_Pages_Should_All_Redirect_To_Login_When_Not_Authenticated**: Tests all protected pages (/departments, /engagements, /employees, /clients, /workitems, /jobpositions)
- **Return_Url_Should_Be_Preserved_When_Redirected_To_Login**: Tests that original URL is preserved as ReturnUrl parameter
- **Logout_Page_Should_Be_Accessible**: Verifies logout functionality redirects to home

#### Authentication Integration Tests (`AuthenticationIntegrationTests.cs`)
- **Login_Page_Contains_Required_Form_Elements**: Validates login form structure at HTTP level
- **Authentication_Flow_Validates_User_Credentials**: Framework for testing credential validation

## Test Configuration

### BUnit Setup
- Mocks for `SignInManager<ApplicationUser>`, `UserManager<ApplicationUser>`, and `IExceptionHandlingService`
- Custom `MockNavigationManager` for testing navigation behavior
- Configured service container with required dependencies

### E2E Test Setup
- Uses `WebApplicationFactory<Program>` for integration testing
- Tests HTTP responses and redirections
- Validates URL parameters and form elements

## Running the Tests

### BUnit Tests
```bash
cd src/tests/EastSeat.ResourceIdea.Web.UnitTests
dotnet test --filter "LoginComponentTests|LogoutComponentTests"
```

### E2E Tests
```bash
cd src/tests/EastSeat.ResourceIdea.Web.E2ETests
dotnet test
```

### All Tests
```bash
dotnet test
```

## Future Enhancements

### Playwright Browser Tests
The E2E project includes infrastructure for Playwright browser tests but focuses on HTTP-level testing due to browser installation constraints in the current environment. To enable full browser testing:

1. Install Playwright browsers:
   ```bash
   playwright install
   ```

2. Uncomment and modify the Playwright test methods in `AuthenticationE2ETests.cs`

### Additional Test Scenarios
- Form validation testing with invalid data
- Session timeout testing
- Multiple concurrent authentication attempts
- Password complexity validation
- Remember me functionality (if implemented)

## Coverage

The current test suite covers:
- ✅ Component rendering and structure
- ✅ Navigation and redirects
- ✅ Authentication state management
- ✅ Protected route access control
- ✅ Return URL functionality
- ✅ Logout workflow

This comprehensive testing approach ensures that authentication functionality works correctly at both the component and integration levels.
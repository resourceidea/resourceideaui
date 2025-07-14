# ResourceIdea E2E Tests

This project contains end-to-end tests for the ResourceIdea web application using Playwright.

## Overview

The E2E tests cover the following UI operations:
- **Client Management**
  - Adding a new client
  - Updating an existing client
  - Form validation for client data
- **Engagement Management**
  - Adding a new client engagement
  - Updating an existing engagement
  - Form validation for engagement data

## Prerequisites

1. .NET 9.0 SDK
2. Playwright browsers installed

## Setup

### 1. Install Playwright Browsers

After building the project, install the required browsers:

```bash
# Build the test project first
dotnet build

# Install Playwright browsers
pwsh bin/Debug/net9.0/playwright.ps1 install
```

Or install only Chromium:
```bash
pwsh bin/Debug/net9.0/playwright.ps1 install chromium
```

### 2. Alternative Browser Installation

If the above doesn't work, you can try:

```bash
# Using PowerShell Core
pwsh -c "bin/Debug/net9.0/playwright.ps1 install"

# Or using the Playwright CLI directly
npx playwright install
```

## Running Tests

### Run All E2E Tests
```bash
dotnet test
```

### Run Specific Test Classes
```bash
# Run only client management tests
dotnet test --filter "FullyQualifiedName~ClientManagementTests"

# Run only engagement management tests  
dotnet test --filter "FullyQualifiedName~EngagementManagementTests"
```

### Run Tests with Verbose Output
```bash
dotnet test --logger "console;verbosity=detailed"
```

## Test Structure

### BaseE2ETest
- Base class providing common setup for all E2E tests
- Manages WebApplicationFactory for hosting the application
- Handles Playwright browser initialization
- Provides helper methods for navigation

### ClientManagementTests
- `AddClient_WithValidData_ShouldSucceed`: Tests successful client creation
- `AddClient_WithEmptyRequiredFields_ShouldShowValidationErrors`: Tests form validation
- `EditClient_WithValidData_ShouldSucceed`: Tests client updates
- `NavigateToAddClient_ShouldDisplayCorrectForm`: Tests form elements are present

### EngagementManagementTests
- `AddEngagement_WithValidData_ShouldSucceed`: Tests successful engagement creation
- `AddEngagement_WithoutSelectingClient_ShouldShowValidationError`: Tests required field validation
- `EditEngagement_WithValidData_ShouldSucceed`: Tests engagement updates
- `NavigateToAddEngagement_ShouldDisplayCorrectForm`: Tests form elements are present
- `AddEngagement_FormValidation_ShouldWorkCorrectly`: Tests overall form validation

## Configuration

The tests use the following configuration:
- **Browser**: Chromium (headless mode)
- **Application**: Runs using WebApplicationFactory with test host
- **Base URL**: Uses the test server's base address

## Troubleshooting

### Browser Installation Issues
If browser installation fails, try:
1. Ensure you have sufficient disk space
2. Check internet connectivity
3. Try installing browsers manually with `npx playwright install`

### Test Failures
- Check that the web application builds successfully
- Verify database connections and dependencies are properly configured
- Ensure test data is available (some tests may require existing clients for editing)

### Performance
- Tests run with appropriate timeouts (5 seconds for page loads, 2 seconds for operations)
- Consider increasing timeouts if running on slower systems

## Notes

- Tests are designed to be independent and can run in parallel
- Some tests may be skipped if required test data is not available
- The tests assume the standard ResourceIdea UI structure and element IDs
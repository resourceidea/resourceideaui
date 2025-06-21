# Centralized Exception Handling

This document describes the centralized exception handling implementation for the ResourceIdea Web application.

## Overview

The application now implements centralized exception handling to:
- Reduce code duplication across components
- Provide consistent error messages to users
- Improve logging and monitoring
- Follow ASP.NET best practices

## Components

### 1. IExceptionHandlingService

A service interface that provides methods for handling exceptions consistently across the application.

**Key Methods:**
- `HandleExceptionAsync(Exception, string?)` - Handles an exception and returns a user-friendly message
- `ExecuteAsync(Func<Task>, string)` - Executes an operation with automatic exception handling
- `ExecuteAsync<T>(Func<Task<T>>, string)` - Executes an operation and returns a result with exception handling

### 2. ExceptionHandlingService

The implementation of `IExceptionHandlingService` that:
- Maps specific exception types to user-friendly error messages
- Logs exceptions appropriately based on severity
- Handles domain-specific exceptions from the ResourceIdea domain

### 3. GlobalExceptionHandlingMiddleware

Middleware that catches unhandled exceptions at the application level and:
- Returns appropriate HTTP status codes
- Provides structured error responses for API endpoints
- Redirects to error pages for web requests
- Logs unhandled exceptions

### 4. ResourceIdeaComponentBase

A base component class that provides:
- Built-in exception handling capabilities
- Loading state management
- Error state management
- Helper methods for executing operations with exception handling

## Usage

### For New Components

Inherit from `ResourceIdeaComponentBase` instead of `ComponentBase`:

```csharp
public partial class MyComponent : ResourceIdeaComponentBase
{
    private async Task LoadData()
    {
        await ExecuteAsync(async () =>
        {
            // Your operation here
            var result = await SomeService.GetDataAsync();
            // Process result
        }, "Loading data");
    }
}
```

### For Operations with Return Values

```csharp
private async Task<SomeData?> LoadSpecificData()
{
    return await ExecuteAsync(async () =>
    {
        var response = await Mediator.Send(query);
        if (response.IsSuccess)
        {
            return response.Content.Value;
        }
        throw new InvalidOperationException("Failed to load data");
    }, "Loading specific data");
}
```

### Manual Exception Handling

If you need custom handling, use the service directly:

```csharp
try
{
    // Your operation
}
catch (Exception ex)
{
    await HandleExceptionAsync(ex, "Custom operation context");
}
```

## Error Messages

The service automatically maps exception types to user-friendly messages:

- `TaskCanceledException` → "The operation was canceled. Please try again."
- `ArgumentException` → "An error occurred due to invalid input. Please check your parameters."
- `InvalidOperationException` → "An invalid operation occurred. Please try again."
- `UnauthorizedAccessException` → "You don't have permission to perform this operation."
- Domain exceptions (e.g., `InvalidEntityIdException`) → Specific domain error messages
- Generic exceptions → "An unexpected error occurred. Please try again or contact support if the problem persists."

## Configuration

The centralized exception handling is registered in `Program.cs`:

```csharp
// Register the service
builder.Services.AddScoped<IExceptionHandlingService, ExceptionHandlingService>();

// Add global middleware
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
```

## Benefits

1. **Consistency**: All components handle exceptions the same way
2. **Maintainability**: Exception handling logic is centralized and easy to update
3. **User Experience**: Users get consistent, friendly error messages
4. **Monitoring**: All exceptions are logged consistently
5. **Best Practices**: Follows ASP.NET Core exception handling patterns

## Migration from Legacy Code

To migrate existing components:

1. Change the base class from `ComponentBase` to `ResourceIdeaComponentBase`
2. Replace try-catch blocks with `ExecuteAsync` method calls
3. Remove manual exception handling code
4. Remove custom loading state management (handled by base class)

Example migration:

**Before:**
```csharp
private async Task LoadData()
{
    IsLoading = true;
    try
    {
        var result = await SomeService.GetDataAsync();
        // Process result
    }
    catch (Exception ex)
    {
        ErrorMessage = $"Error: {ex.Message}";
        HasError = true;
    }
    finally
    {
        IsLoading = false;
        StateHasChanged();
    }
}
```

**After:**
```csharp
private async Task LoadData()
{
    await ExecuteAsync(async () =>
    {
        var result = await SomeService.GetDataAsync();
        // Process result
    }, "Loading data");
}
```

## Testing

Unit tests are provided in `ExceptionHandlingServiceTests.cs` to verify:
- Correct error message mapping
- Proper exception logging
- Operation execution with and without exceptions
- Result handling for operations that return data
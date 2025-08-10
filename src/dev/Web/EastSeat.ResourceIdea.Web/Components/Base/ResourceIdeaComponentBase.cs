using EastSeat.ResourceIdea.Web.Services;
using EastSeat.ResourceIdea.Web.Exceptions;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Base;

/// <summary>
/// Base component class that provides centralized exception handling capabilities.
/// </summary>
public abstract class ResourceIdeaComponentBase : ComponentBase, IDisposable
{
    [Inject] protected IExceptionHandlingService ExceptionHandlingService { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;

    /// <summary>
    /// Cancellation token source for component lifecycle management.
    /// </summary>
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private bool _disposed = false;

    /// <summary>
    /// Cancellation token that is cancelled when the component is disposed.
    /// </summary>
    protected CancellationToken ComponentCancellationToken
    {
        get
        {
            if (_disposed)
            {
                return CancellationToken.None;
            }
            return _cancellationTokenSource.Token;
        }
    }

    /// <summary>
    /// Indicates if the component has an error state.
    /// </summary>
    protected bool HasError { get; set; }

    /// <summary>
    /// The current error message to display to the user.
    /// </summary>
    protected string? ErrorMessage { get; set; }

    /// <summary>
    /// Indicates if the component is currently loading.
    /// </summary>
    protected bool IsLoading { get; set; }

    /// <summary>
    /// Indicates if the page is currently loading (alias for IsLoading for backward compatibility).
    /// </summary>
    protected bool IsLoadingPage
    {
        get => IsLoading;
        set => IsLoading = value;
    }

    /// <summary>
    /// Executes an operation with centralized exception handling and cancellation token support.
    /// Automatically manages loading state and error handling.
    /// </summary>
    /// <param name="operation">The operation to execute</param>
    /// <param name="context">Context information about the operation</param>
    /// <param name="manageLoadingState">Whether to automatically manage loading state</param>
    /// <param name="cancellationToken">Optional cancellation token (uses component token if not provided)</param>
    /// <returns>True if operation succeeded, false otherwise</returns>
    protected async Task<bool> ExecuteAsync(Func<CancellationToken, Task> operation, string context, bool manageLoadingState = true, CancellationToken cancellationToken = default)
    {
        var effectiveToken = cancellationToken == default ? ComponentCancellationToken : cancellationToken;

        if (manageLoadingState)
        {
            IsLoading = true;
            SafeStateHasChanged();
        }

        try
        {
            ClearError();

            await operation(effectiveToken);

            // Check if operation was cancelled
            if (effectiveToken.IsCancellationRequested)
            {
                return false;
            }

            return true;
        }
        catch (OperationCanceledException)
        {
            // Operation was cancelled, this is expected behavior
            return false;
        }
        catch (Exception ex)
        {
            var result = await ExceptionHandlingService.ExecuteAsync(() => throw ex, context);

            if (!result.IsSuccess)
            {
                // Handle tenant authentication exceptions by redirecting to login
                if (result.Exception is TenantAuthenticationException)
                {
                    RedirectToLogin();
                    return false;
                }

                SetError(result.ErrorMessage!);
                return false;
            }

            return true;
        }
        finally
        {
            if (manageLoadingState)
            {
                IsLoading = false;
                SafeStateHasChanged();
            }
        }
    }

    /// <summary>
    /// Executes an operation with centralized exception handling (backward compatibility).
    /// </summary>
    /// <param name="operation">The operation to execute</param>
    /// <param name="context">Context information about the operation</param>
    /// <param name="manageLoadingState">Whether to automatically manage loading state</param>
    /// <returns>True if operation succeeded, false otherwise</returns>
    protected async Task<bool> ExecuteAsync(Func<Task> operation, string context, bool manageLoadingState = true)
    {
        return await ExecuteAsync(_ => operation(), context, manageLoadingState);
    }

    /// <summary>
    /// Executes an operation with centralized exception handling and returns a result.
    /// Automatically manages loading state and error handling.
    /// </summary>
    /// <typeparam name="T">The type of result returned by the operation</typeparam>
    /// <param name="operation">The operation to execute</param>
    /// <param name="context">Context information about the operation</param>
    /// <param name="manageLoadingState">Whether to automatically manage loading state</param>
    /// <param name="cancellationToken">Optional cancellation token (uses component token if not provided)</param>
    /// <returns>The result of the operation, or default(T) if failed</returns>
    protected async Task<T?> ExecuteAsync<T>(Func<CancellationToken, Task<T>> operation, string context, bool manageLoadingState = true, CancellationToken cancellationToken = default)
    {
        var effectiveToken = cancellationToken == default ? ComponentCancellationToken : cancellationToken;

        if (manageLoadingState)
        {
            IsLoading = true;
            SafeStateHasChanged();
        }

        try
        {
            ClearError();

            var result = await operation(effectiveToken);

            // Check if operation was cancelled
            if (effectiveToken.IsCancellationRequested)
            {
                return default;
            }

            return result;
        }
        catch (OperationCanceledException)
        {
            // Operation was cancelled, this is expected behavior
            return default;
        }
        catch (Exception ex)
        {
            var result = await ExceptionHandlingService.ExecuteAsync(() => throw ex, context);

            if (!result.IsSuccess)
            {
                // Handle tenant authentication exceptions by redirecting to login
                if (result.Exception is TenantAuthenticationException)
                {
                    RedirectToLogin();
                    return default;
                }

                SetError(result.ErrorMessage!);
                return default;
            }

            return default;
        }
        finally
        {
            if (manageLoadingState)
            {
                IsLoading = false;
                SafeStateHasChanged();
            }
        }
    }

    /// <summary>
    /// Executes an operation with centralized exception handling and returns a result (backward compatibility).
    /// </summary>
    /// <typeparam name="T">The type of result returned by the operation</typeparam>
    /// <param name="operation">The operation to execute</param>
    /// <param name="context">Context information about the operation</param>
    /// <param name="manageLoadingState">Whether to automatically manage loading state</param>
    /// <returns>The result of the operation, or default(T) if failed</returns>
    protected async Task<T?> ExecuteAsync<T>(Func<Task<T>> operation, string context, bool manageLoadingState = true)
    {
        return await ExecuteAsync(_ => operation(), context, manageLoadingState);
    }

    /// <summary>
    /// Redirects to the login page with the current URL as return URL.
    /// </summary>
    private void RedirectToLogin()
    {
        var returnUrl = Navigation.ToBaseRelativePath(Navigation.Uri);
        var loginUrl = $"/login?returnUrl={Uri.EscapeDataString(returnUrl)}";
        Navigation.NavigateTo(loginUrl, true);
    }

    /// <summary>
    /// Sets an error message and updates the component state.
    /// </summary>
    /// <param name="message">The error message to display</param>
    protected void SetError(string message)
    {
        HasError = true;
        ErrorMessage = message;
        SafeStateHasChanged();
    }

    /// <summary>
    /// Clears any current error state.
    /// </summary>
    protected void ClearError()
    {
        HasError = false;
        ErrorMessage = null;
    }

    /// <summary>
    /// Helper method to minimize StateHasChanged calls for better performance.
    /// Only calls StateHasChanged if the component hasn't been disposed.
    /// </summary>
    protected void SafeStateHasChanged()
    {
        if (!_disposed && !ComponentCancellationToken.IsCancellationRequested)
        {
            try
            {
                StateHasChanged();
            }
            catch (ObjectDisposedException)
            {
                // Component has been disposed
                // This is safe to ignore
            }
            catch (InvalidOperationException)
            {
                // Component may have been disposed during rendering
                // This is safe to ignore
            }
        }
    }

    /// <summary>
    /// Handles exceptions that occur outside of the ExecuteAsync methods.
    /// Use this for legacy exception handling or special cases.
    /// </summary>
    /// <param name="exception">The exception to handle</param>
    /// <param name="context">Context information about where the exception occurred</param>
    protected async Task HandleExceptionAsync(Exception exception, string? context = null)
    {
        var errorMessage = await ExceptionHandlingService.HandleExceptionAsync(exception, this, context);
        SetError(errorMessage);
    }

    /// <summary>
    /// Dispose method for cleanup.
    /// </summary>
    public virtual void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;

            // Cancel any ongoing operations
            try
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // Already disposed, ignore
            }
        }
    }
}
using EastSeat.ResourceIdea.Web.Services;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Components.Base;

/// <summary>
/// Base component class that provides centralized exception handling capabilities.
/// </summary>
public abstract class ResourceIdeaComponentBase : ComponentBase, IDisposable
{
    [Inject] protected IExceptionHandlingService ExceptionHandlingService { get; set; } = default!;

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
    /// Executes an operation with centralized exception handling.
    /// Automatically manages loading state and error handling.
    /// </summary>
    /// <param name="operation">The operation to execute</param>
    /// <param name="context">Context information about the operation</param>
    /// <param name="manageLoadingState">Whether to automatically manage loading state</param>
    /// <returns>True if operation succeeded, false otherwise</returns>
    protected async Task<bool> ExecuteAsync(Func<Task> operation, string context, bool manageLoadingState = true)
    {
        if (manageLoadingState)
        {
            IsLoading = true;
            StateHasChanged();
        }

        try
        {
            ClearError();
            
            var result = await ExceptionHandlingService.ExecuteAsync(operation, context);
            
            if (!result.IsSuccess)
            {
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
                StateHasChanged();
            }
        }
    }

    /// <summary>
    /// Executes an operation with centralized exception handling and returns a result.
    /// Automatically manages loading state and error handling.
    /// </summary>
    /// <typeparam name="T">The type of result returned by the operation</typeparam>
    /// <param name="operation">The operation to execute</param>
    /// <param name="context">Context information about the operation</param>
    /// <param name="manageLoadingState">Whether to automatically manage loading state</param>
    /// <returns>The result of the operation, or default(T) if failed</returns>
    protected async Task<T?> ExecuteAsync<T>(Func<Task<T>> operation, string context, bool manageLoadingState = true)
    {
        if (manageLoadingState)
        {
            IsLoading = true;
            StateHasChanged();
        }

        try
        {
            ClearError();
            
            var result = await ExceptionHandlingService.ExecuteAsync(operation, context);
            
            if (!result.IsSuccess)
            {
                SetError(result.ErrorMessage!);
                return default;
            }

            return result.Data;
        }
        finally
        {
            if (manageLoadingState)
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
    }

    /// <summary>
    /// Sets an error message and updates the component state.
    /// </summary>
    /// <param name="message">The error message to display</param>
    protected void SetError(string message)
    {
        HasError = true;
        ErrorMessage = message;
        StateHasChanged();
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
        // Base implementation - override in derived classes if needed
    }
}
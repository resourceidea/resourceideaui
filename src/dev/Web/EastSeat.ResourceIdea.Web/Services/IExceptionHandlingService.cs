using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Services;

/// <summary>
/// Service for centralized exception handling in components.
/// </summary>
public interface IExceptionHandlingService
{
    /// <summary>
    /// Handles exceptions that occur in component operations.
    /// </summary>
    /// <param name="exception">The exception to handle</param>
    /// <param name="context">Optional context information about where the exception occurred</param>
    /// <returns>A user-friendly error message</returns>
    Task<string> HandleExceptionAsync(Exception exception, string? context = null);

    /// <summary>
    /// Handles exceptions that occur in component operations with state change callback.
    /// </summary>
    /// <param name="exception">The exception to handle</param>
    /// <param name="component">The component where the exception occurred</param>
    /// <param name="context">Optional context information about where the exception occurred</param>
    /// <returns>A user-friendly error message</returns>
    Task<string> HandleExceptionAsync(Exception exception, ComponentBase component, string? context = null);

    /// <summary>
    /// Executes an operation with centralized exception handling.
    /// </summary>
    /// <param name="operation">The operation to execute</param>
    /// <param name="context">Context information about the operation</param>
    /// <returns>Result containing success status and error message if any</returns>
    Task<ExceptionHandlingResult> ExecuteAsync(Func<Task> operation, string context);

    /// <summary>
    /// Executes an operation with centralized exception handling and returns a result.
    /// </summary>
    /// <typeparam name="T">The type of result returned by the operation</typeparam>
    /// <param name="operation">The operation to execute</param>
    /// <param name="context">Context information about the operation</param>
    /// <returns>Result containing success status, data, and error message if any</returns>
    Task<ExceptionHandlingResult<T>> ExecuteAsync<T>(Func<Task<T>> operation, string context);
}

/// <summary>
/// Result of an operation with exception handling.
/// </summary>
public class ExceptionHandlingResult
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public Exception? Exception { get; init; }

    public static ExceptionHandlingResult Success() => new() { IsSuccess = true };
    public static ExceptionHandlingResult Failure(string errorMessage, Exception? exception = null) => 
        new() { IsSuccess = false, ErrorMessage = errorMessage, Exception = exception };
}

/// <summary>
/// Result of an operation with exception handling that returns data.
/// </summary>
/// <typeparam name="T">The type of data returned</typeparam>
public class ExceptionHandlingResult<T> : ExceptionHandlingResult
{
    public T? Data { get; init; }

    public static ExceptionHandlingResult<T> Success(T data) => 
        new() { IsSuccess = true, Data = data };
    
    public static new ExceptionHandlingResult<T> Failure(string errorMessage, Exception? exception = null) => 
        new() { IsSuccess = false, ErrorMessage = errorMessage, Exception = exception };
}
using EastSeat.ResourceIdea.Domain.Exceptions;
using EastSeat.ResourceIdea.Web.Exceptions;
using Microsoft.AspNetCore.Components;

namespace EastSeat.ResourceIdea.Web.Services;

/// <summary>
/// Implementation of centralized exception handling service.
/// </summary>
public class ExceptionHandlingService : IExceptionHandlingService
{
    private readonly ILogger<ExceptionHandlingService> _logger;

    public ExceptionHandlingService(ILogger<ExceptionHandlingService> logger)
    {
        _logger = logger;
    }

    public async Task<string> HandleExceptionAsync(Exception exception, string? context = null)
    {
        var errorMessage = GetUserFriendlyErrorMessage(exception);
        await LogExceptionAsync(exception, context);
        return errorMessage;
    }

    public async Task<string> HandleExceptionAsync(Exception exception, ComponentBase component, string? context = null)
    {
        var errorMessage = await HandleExceptionAsync(exception, context);

        // Component state change is handled by the ResourceIdeaComponentBase
        return errorMessage;
    }

    public async Task<ExceptionHandlingResult> ExecuteAsync(Func<Task> operation, string context)
    {
        try
        {
            await operation();
            return ExceptionHandlingResult.Success();
        }
        catch (TaskCanceledException ex)
        {
            var errorMessage = await HandleExceptionAsync(ex, context);
            return ExceptionHandlingResult.Failure(errorMessage, ex);
        }
        catch (TimeoutException ex)
        {
            var errorMessage = await HandleExceptionAsync(ex, context);
            return ExceptionHandlingResult.Failure(errorMessage, ex);
        }
        catch (ArgumentException ex)
        {
            var errorMessage = await HandleExceptionAsync(ex, context);
            return ExceptionHandlingResult.Failure(errorMessage, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            var errorMessage = await HandleExceptionAsync(ex, context);
            return ExceptionHandlingResult.Failure(errorMessage, ex);
        }
        catch (TenantAuthenticationException ex)
        {
            var errorMessage = await HandleExceptionAsync(ex, context);
            return ExceptionHandlingResult.Failure(errorMessage, ex);
        }
        catch (ResourceIdeaException ex)
        {
            var errorMessage = await HandleExceptionAsync(ex, context);
            return ExceptionHandlingResult.Failure(errorMessage, ex);
        }
        catch (Exception ex)
        {
            // Rethrow critical exceptions
            if (ex is OutOfMemoryException || ex is StackOverflowException || ex is ThreadAbortException)
            {
                throw;
            }

            var errorMessage = await HandleExceptionAsync(ex, context);
            return ExceptionHandlingResult.Failure(errorMessage, ex);
        }
    }

    public async Task<ExceptionHandlingResult<T>> ExecuteAsync<T>(Func<Task<T>> operation, string context)
    {
        try
        {
            var result = await operation();
            return ExceptionHandlingResult<T>.Success(result);
        }
        catch (TaskCanceledException ex)
        {
            var errorMessage = await HandleExceptionAsync(ex, context);
            return ExceptionHandlingResult<T>.Failure(errorMessage, ex);
        }
        catch (TimeoutException ex)
        {
            var errorMessage = await HandleExceptionAsync(ex, context);
            return ExceptionHandlingResult<T>.Failure(errorMessage, ex);
        }
        catch (ArgumentException ex)
        {
            var errorMessage = await HandleExceptionAsync(ex, context);
            return ExceptionHandlingResult<T>.Failure(errorMessage, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            var errorMessage = await HandleExceptionAsync(ex, context);
            return ExceptionHandlingResult<T>.Failure(errorMessage, ex);
        }
        catch (TenantAuthenticationException ex)
        {
            var errorMessage = await HandleExceptionAsync(ex, context);
            return ExceptionHandlingResult<T>.Failure(errorMessage, ex);
        }
        catch (ResourceIdeaException ex)
        {
            var errorMessage = await HandleExceptionAsync(ex, context);
            return ExceptionHandlingResult<T>.Failure(errorMessage, ex);
        }
        catch (Exception ex)
        {
            // Rethrow critical exceptions
            if (ex is OutOfMemoryException || ex is StackOverflowException || ex is ThreadAbortException)
            {
                throw;
            }

            var errorMessage = await HandleExceptionAsync(ex, context);
            return ExceptionHandlingResult<T>.Failure(errorMessage, ex);
        }
    }

    private string GetUserFriendlyErrorMessage(Exception exception)
    {
        return exception switch
        {
            TaskCanceledException => "The operation was canceled. Please try again.",
            TimeoutException => "The operation timed out. Please try again.",
            ArgumentNullException => "Required information is missing. Please provide all necessary details.",
            ArgumentException => "An error occurred due to invalid input. Please check your parameters.",
            InvalidOperationException => "An invalid operation occurred. Please try again.",
            UnauthorizedAccessException => "You don't have permission to perform this operation.",
            TenantAuthenticationException => exception.Message, // Use the specific message for tenant auth issues
            ResourceIdeaException domain => GetDomainExceptionMessage(domain),
            HttpRequestException => "A network error occurred. Please check your connection and try again.",
            NotSupportedException => "This operation is not supported.",
            _ => "An unexpected error occurred. Please try again or contact support if the problem persists."
        };
    }

    private static string GetDomainExceptionMessage(ResourceIdeaException exception)
    {
        // Handle specific domain exceptions with more user-friendly messages
        return exception switch
        {
            InvalidEntityIdException => "The provided identifier is invalid.",
            UpdateItemNotFoundException => "The item you're trying to update was not found.",
            _ => "A business rule validation failed. Please check your input and try again."
        };
    }

    private async Task LogExceptionAsync(Exception exception, string? context)
    {
        // Log the exception details for debugging and monitoring
        var logContext = !string.IsNullOrEmpty(context) ? $" in context: {context}" : "";

        switch (exception)
        {
            case TaskCanceledException:
            case TimeoutException:
                _logger.LogWarning(exception, "Operation canceled or timed out{Context}", logContext);
                break;

            case ArgumentNullException:
            case ArgumentException:
                _logger.LogWarning(exception, "Invalid argument provided{Context}", logContext);
                break;

            case UnauthorizedAccessException:
                _logger.LogWarning(exception, "Unauthorized access attempt{Context}", logContext);
                break;

            case TenantAuthenticationException:
                _logger.LogWarning(exception, "Tenant authentication issue{Context}", logContext);
                break;

            case ResourceIdeaException:
                _logger.LogWarning(exception, "Domain exception occurred{Context}", logContext);
                break;

            default:
                _logger.LogError(exception, "Unexpected exception occurred{Context}", logContext);
                break;
        }

        await Task.CompletedTask;
    }
}
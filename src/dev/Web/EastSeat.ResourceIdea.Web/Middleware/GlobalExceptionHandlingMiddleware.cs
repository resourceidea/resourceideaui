using EastSeat.ResourceIdea.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace EastSeat.ResourceIdea.Web.Middleware;

/// <summary>
/// Global exception handling middleware for unhandled exceptions.
/// </summary>
public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var sanitizedPath = context.Request.Path.Value?.Replace("\r", "").Replace("\n", "");
            _logger.LogError(ex, "An unhandled exception occurred. Request Path: {Path}", sanitizedPath);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = GetResponseDetails(exception);
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            error = new
            {
                message,
                statusCode = (int)statusCode,
                timestamp = DateTime.UtcNow
            }
        };

        // For API requests, return JSON error response
        if (context.Request.Path.StartsWithSegments("/api") || 
            context.Request.Headers.Accept.Any(h => h?.Contains("application/json") == true))
        {
            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
        else
        {
            // For regular web requests, redirect to error page
            context.Response.ContentType = "text/html";
            context.Response.StatusCode = 500;
            context.Response.Redirect("/Error");
        }
    }

    private static (HttpStatusCode statusCode, string message) GetResponseDetails(Exception exception)
    {
        return exception switch
        {
            ArgumentException or ArgumentNullException => 
                (HttpStatusCode.BadRequest, "Invalid request parameters."),
            
            UnauthorizedAccessException => 
                (HttpStatusCode.Unauthorized, "You are not authorized to perform this operation."),
            
            ResourceIdeaException domain when domain is InvalidEntityIdException => 
                (HttpStatusCode.BadRequest, "Invalid identifier provided."),
            
            ResourceIdeaException domain when domain is UpdateItemNotFoundException => 
                (HttpStatusCode.NotFound, "The requested item was not found."),
            
            ResourceIdeaException => 
                (HttpStatusCode.BadRequest, "A business rule validation failed."),
            
            TaskCanceledException or TimeoutException => 
                (HttpStatusCode.RequestTimeout, "The request timed out."),
            
            HttpRequestException => 
                (HttpStatusCode.BadGateway, "An external service is unavailable."),
            
            NotImplementedException => 
                (HttpStatusCode.NotImplemented, "This feature is not yet implemented."),
            
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };
    }
}
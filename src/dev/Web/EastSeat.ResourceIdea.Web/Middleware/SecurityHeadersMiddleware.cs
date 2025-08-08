using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EastSeat.ResourceIdea.Web.Middleware;

/// <summary>
/// Middleware to add security headers to HTTP responses following Microsoft Blazor security best practices.
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Add Content Security Policy (CSP) to protect against XSS attacks
        if (!context.Response.Headers.ContainsKey("Content-Security-Policy"))
        {
            context.Response.Headers["Content-Security-Policy"] =
                "default-src 'self'; " +
                "script-src 'self' 'unsafe-inline' 'unsafe-eval' cdn.jsdelivr.net kit.fontawesome.com; " +
                "style-src 'self' 'unsafe-inline' cdn.jsdelivr.net; " +
                "font-src 'self' kit.fontawesome.com ka-f.fontawesome.com; " +
                "img-src 'self' data:; " +
                "connect-src 'self'; " +
                "frame-ancestors 'none'; " +
                "base-uri 'self'; " +
                "form-action 'self';";
        }

        // Add X-Frame-Options to protect against click-jacking
        if (!context.Response.Headers.ContainsKey("X-Frame-Options"))
        {
            context.Response.Headers["X-Frame-Options"] = "DENY";
        }

        // Add X-Content-Type-Options to prevent MIME type sniffing
        if (!context.Response.Headers.ContainsKey("X-Content-Type-Options"))
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        }

        // Add Referrer-Policy for privacy protection
        if (!context.Response.Headers.ContainsKey("Referrer-Policy"))
        {
            context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
        }

        // Add Permissions-Policy to restrict access to browser features
        if (!context.Response.Headers.ContainsKey("Permissions-Policy"))
        {
            context.Response.Headers["Permissions-Policy"] =
                "camera=(), microphone=(), geolocation=(), payment=()";
        }

        // Add X-XSS-Protection for legacy browser support
        if (!context.Response.Headers.ContainsKey("X-XSS-Protection"))
        {
            context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
        }

        await _next(context);
    }
}

namespace ResourceIdea.Middleware;

/// <summary>
/// Middleware to check presence of user subscription code.
/// </summary>
public class CheckSubscriptionMiddleware
{
    private readonly RequestDelegate _next;

    public CheckSubscriptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async System.Threading.Tasks.Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.User.Identity!.IsAuthenticated && 
            httpContext.Request.Cookies["CompanyCode"] is null)
        {
            httpContext.Response.Redirect("/logout");
        }

        await _next(httpContext);
    }
}

public static class CheckSubscriptionMiddlewareExtensions
{
    public static IApplicationBuilder UseCheckSubscriptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CheckSubscriptionMiddleware>();
    }
}
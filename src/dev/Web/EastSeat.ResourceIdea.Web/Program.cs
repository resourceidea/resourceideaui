using EastSeat.ResourceIdea.Web;
using EastSeat.ResourceIdea.Web.Components;
using EastSeat.ResourceIdea.Application.Features.Departments.Handlers;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.DataStore;
using Microsoft.AspNetCore.Identity;
using EastSeat.ResourceIdea.Web.Services;
using EastSeat.ResourceIdea.Web.Middleware;
using EastSeat.ResourceIdea.Web.Extensions;
using EastSeat.ResourceIdea.Web.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add Azure Monitor OpenTelemetry
builder.Services.AddResourceIdeaTelemetry(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IResourceIdeaRequestContext, ResourceIdeaRequestContext>();
builder.Services.AddMemoryCache();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddResourceIdeaDbContext();
builder.Services.AddResourceIdeaServices();

// Configure Identity services with SignInManager
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Password settings
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ResourceIdeaDBContext>()
.AddDefaultTokenProviders();

// Add claims transformation to inject TenantId and backend role claims
builder.Services.AddScoped<IClaimsTransformation, TenantClaimsTransformation>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/auth/signout";
    options.AccessDeniedPath = "/access-denied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8); // Reduced from 24 to 8 hours
    options.SlidingExpiration = false; // Disable sliding expiration to prevent auto-renewal
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Lax;

    // Force session-only cookies (not persistent)
    options.Events.OnSigningIn = context =>
    {
        // Ensure cookies are session-only unless explicitly set as persistent
        if (context.Properties.IsPersistent == false)
        {
            context.CookieOptions.Expires = null;
            context.CookieOptions.MaxAge = null;
        }
        return Task.CompletedTask;
    };
});

// Add authorization services with backend access policy
builder.Services.AddAuthorizationCore(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    // Policy for backend access (Developer and Support roles)
    options.AddPolicy("BackendAccess", policy =>
        policy.Requirements.Add(new BackendAccessRequirement()));

    // Policy for tenant access (regular tenant users)
    options.AddPolicy("TenantAccess", policy =>
        policy.RequireAuthenticatedUser()
        .RequireAssertion(context =>
        {
            // Deny access if user has backend role
            var isBackendRole = context.User.FindFirst("IsBackendRole")?.Value;
            return !bool.TryParse(isBackendRole, out bool isBackend) || !isBackend;
        }));
});

// Register authorization handlers
builder.Services.AddScoped<IAuthorizationHandler, BackendAccessHandler>();

// Add MediatR
builder.Services.AddMediatR(typeof(CreateDepartmentCommandHandler).Assembly);

// Add centralized exception handling service
builder.Services.AddScoped<IExceptionHandlingService, ExceptionHandlingService>();

// Add notification service
builder.Services.AddScoped<NotificationService>();

// Add health checks for Azure App Service monitoring
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ResourceIdeaDBContext>("database");

// Add rate limiting for authentication endpoints
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        // Apply stricter limits to authentication endpoints
        var endpoint = context.Request.Path.Value?.ToLowerInvariant();
        if (endpoint?.Contains("/login") == true || endpoint?.Contains("/logout") == true)
        {
            return RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 5, // 5 attempts
                    Window = TimeSpan.FromMinutes(1), // per minute
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 2
                });
        }

        // Default rate limit for other endpoints
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100, // 100 requests
                Window = TimeSpan.FromMinutes(1), // per minute
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 10
            });
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429; // Too Many Requests
        await context.HttpContext.Response.WriteAsync("Rate limit exceeded. Please try again later.", token);
    };
});

var app = builder.Build();

// Add global exception handling middleware
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

// Add security headers middleware
app.UseMiddleware<SecurityHeadersMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.UseStaticFiles();

// Add health check endpoint for Azure App Service monitoring
app.MapHealthChecks("/health");

// GET fallback for signout: renders an auto-submitting form that POSTs with antiforgery token
// This supports redirects like "/auth/signout?returnUrl=%2F" by immediately posting to the
// POST endpoint below while preserving CSRF protection.
app.MapGet("/auth/signout", (HttpContext http, IServiceProvider serviceProvider, string? returnUrl) =>
{
    var antiforgery = serviceProvider.GetRequiredService<Microsoft.AspNetCore.Antiforgery.IAntiforgery>();
    var tokens = antiforgery.GetAndStoreTokens(http);

    // Ensure local-only returnUrl
    var target = "/";
    if (!string.IsNullOrWhiteSpace(returnUrl)
            && returnUrl.StartsWith("/", StringComparison.Ordinal)
            && !returnUrl.StartsWith("//", StringComparison.Ordinal))
    {
        target = returnUrl;
    }

    var html = "<!doctype html>\n" +
               "<html>\n" +
               "  <head>\n" +
               "    <meta charset=\"utf-8\" />\n" +
               "    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />\n" +
               "    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\" />\n" +
               "    <title>Signing out…</title>\n" +
               "  </head>\n" +
               "  <body>\n" +
               $"    <form id=\"f\" method=\"post\" action=\"/auth/signout\">\n" +
               $"      <input type=\"hidden\" name=\"{tokens.FormFieldName}\" value=\"{tokens.RequestToken}\" />\n" +
               $"      <input type=\"hidden\" name=\"returnUrl\" value=\"{System.Net.WebUtility.HtmlEncode(target)}\" />\n" +
               "      <noscript>\n" +
               "        <p>JavaScript is required to complete sign out. Click the button below.</p>\n" +
               "        <button type=\"submit\">Sign out</button>\n" +
               "      </noscript>\n" +
               "    </form>\n" +
               "    <script>document.getElementById('f').submit();</script>\n" +
               "  </body>\n" +
               "</html>";

    return Results.Content(html, "text/html; charset=utf-8");
});

// Minimal API endpoint to sign out and redirect, using POST with antiforgery and local-only returnUrl
app.MapPost("/auth/signout", async (HttpContext http, IServiceProvider serviceProvider, string? returnUrl) =>
{
    // Validate antiforgery token
    var antiforgery = serviceProvider.GetRequiredService<Microsoft.AspNetCore.Antiforgery.IAntiforgery>();
    await antiforgery.ValidateRequestAsync(http);

    await http.SignOutAsync(IdentityConstants.ApplicationScheme);
    var target = "/";
    if (!string.IsNullOrWhiteSpace(returnUrl)
        && returnUrl.StartsWith("/", StringComparison.Ordinal)
        && !returnUrl.StartsWith("//", StringComparison.Ordinal))
    {
        target = returnUrl;
    }
    return Results.Redirect(target);
});

app.MapRazorComponents<EastSeat.ResourceIdea.Web.Components.App>()
   .AddInteractiveServerRenderMode();

// Run database startup tasks (migrations, seed data, etc.)
await app.RunDatabaseStartupTasks();

app.Run();

// Make the implicit Program class public for testing
public partial class Program { }

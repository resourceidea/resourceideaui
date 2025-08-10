using EastSeat.ResourceIdea.Web.Components;
using EastSeat.ResourceIdea.Web;
using EastSeat.ResourceIdea.Application.Features.Departments.Handlers;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.DataStore;
using Microsoft.AspNetCore.Identity;
using EastSeat.ResourceIdea.DataStore.Identity;
using EastSeat.ResourceIdea.Web.Services;
using EastSeat.ResourceIdea.Web.Middleware;
using EastSeat.ResourceIdea.Web.Extensions;
using EastSeat.ResourceIdea.Web.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add Azure Monitor OpenTelemetry
builder.Services.AddResourceIdeaTelemetry(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IResourceIdeaRequestContext, ResourceIdeaRequestContext>();

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
    options.LogoutPath = "/logout";
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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

// Run database startup tasks (migrations, seed data, etc.)
await app.RunDatabaseStartupTasks();

app.Run();

// Make the implicit Program class public for testing
public partial class Program { }

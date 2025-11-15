using EastSeat.ResourceIdea.Application.Common;
using EastSeat.ResourceIdea.Application.Services;
using EastSeat.ResourceIdea.Infrastructure.Data;
using EastSeat.ResourceIdea.Infrastructure.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using EastSeat.ResourceIdea.Server.Components;
using EastSeat.ResourceIdea.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add MudBlazor services
builder.Services.AddMudServices();

// Configure PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;

    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure cookie authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

// Register application services
builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());

// HttpContext accessor + current user service
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<EngagementService>();
builder.Services.AddScoped<PlannerService>();
builder.Services.AddScoped<RolloverService>();

// Register data seeder
builder.Services.AddScoped<DataSeeder>();

// Add authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PolicyNames.AdminOnly, policy =>
        policy.RequireRole(RoleNames.Admin));

    options.AddPolicy(PolicyNames.ManageEngagements, policy =>
        policy.RequireRole(RoleNames.Admin, RoleNames.Partner, RoleNames.Manager));

    options.AddPolicy(PolicyNames.ViewPlanner, policy =>
        policy.RequireAuthenticatedUser());
});

var app = builder.Build();

// Conditional seed (uses SEED_ADMIN_USER env/config). Gracefully skip if database unavailable.
if (builder.Configuration.GetValue<bool>("SEED_ADMIN_USER", false))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (await dbContext.Database.CanConnectAsync())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        try
        {
            await seeder.SeedAsync();
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "Data seeding failed but application will continue running.");
        }
    }
    else
    {
        app.Logger.LogWarning("Skipping data seeding: database not reachable.");
    }
}

// Run migrations on startup (development only)
if (app.Environment.IsDevelopment() &&
    builder.Configuration.GetValue<bool>("RunMigrationsOnStartup", false))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (await dbContext.Database.CanConnectAsync())
    {
        await dbContext.Database.MigrateAsync();
    }
    else
    {
        app.Logger.LogWarning("Skipping migrations: database not reachable.");
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

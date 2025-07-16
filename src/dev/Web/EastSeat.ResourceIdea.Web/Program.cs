using EastSeat.ResourceIdea.Web.Components;
using EastSeat.ResourceIdea.Web;
using EastSeat.ResourceIdea.Application.Features.Departments.Handlers;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.DataStore;
using Microsoft.AspNetCore.Identity;
using EastSeat.ResourceIdea.DataStore.Identity;
using EastSeat.ResourceIdea.Web.Services;
using EastSeat.ResourceIdea.Web.Middleware;
using EastSeat.ResourceIdea.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Azure Monitor OpenTelemetry
builder.Services.AddResourceIdeaTelemetry(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ResourceIdeaRequestContext>();
builder.Services.AddScoped<IResourceIdeaRequestContext>(provider => provider.GetRequiredService<ResourceIdeaRequestContext>());
builder.Services.AddScoped<IAuthenticationContext>(provider => provider.GetRequiredService<ResourceIdeaRequestContext>());

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddResourceIdeaDbContext();
builder.Services.AddResourceIdeaServices();

builder.Services.AddScoped<IUserStore<ApplicationUser>, CustomUserStore>();

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

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/access-denied";
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;
});

// Add authorization services
builder.Services.AddAuthorizationCore(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDepartmentCommandHandler).Assembly));

// Add centralized exception handling service
builder.Services.AddScoped<IExceptionHandlingService, ExceptionHandlingService>();

var app = builder.Build();

// Add global exception handling middleware
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.UseStaticFiles();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();

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
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IResourceIdeaRequestContext, ResourceIdeaRequestContext>();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// Add controllers for API endpoints
builder.Services.AddControllers();

// Add basic HttpClient support
builder.Services.AddHttpClient();

builder.Services.AddResourceIdeaDbContext();
builder.Services.AddResourceIdeaServices();

// Add Identity with SignInManager and other required services
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Configure password requirements
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;

    // Configure user requirements
    options.User.RequireUniqueEmail = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<ResourceIdeaDBContext>()
.AddDefaultTokenProviders();

// Configure cookie authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/access-denied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
});

// Add authorization services
builder.Services.AddAuthorizationCore(options =>
{
    // Add default policy that requires authentication
    options.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    // Add policy for admin users
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireRole("Admin"));
});



// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDepartmentCommandHandler).Assembly));

// Add centralized exception handling service
builder.Services.AddScoped<IExceptionHandlingService, ExceptionHandlingService>();
// builder.Services.AddScoped<EastSeat.ResourceIdea.Web.Services.UserSeedService>();

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

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.UseStaticFiles();

// Map controllers for API endpoints
app.MapControllers();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

// Run database startup tasks if configured
await app.RunDatabaseStartupTasks();

app.Run();

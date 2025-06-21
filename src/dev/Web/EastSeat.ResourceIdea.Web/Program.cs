using EastSeat.ResourceIdea.Web.Components;
using EastSeat.ResourceIdea.Web;
using EastSeat.ResourceIdea.Application.Features.Departments.Handlers;
using EastSeat.ResourceIdea.Web.RequestContext;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.DataStore;
using Microsoft.AspNetCore.Identity;
using EastSeat.ResourceIdea.DataStore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IResourceIdeaRequestContext, ResourceIdeaRequestContext>();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddResourceIdeaDbContext();
builder.Services.AddResourceIdeaServices();

builder.Services.AddScoped<IUserStore<ApplicationUser>, CustomUserStore>();

// Add this after builder.Services.AddResourceIdeaServices();
builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ResourceIdeaDBContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies(options =>
{
    options.ApplicationCookie?.Configure(config =>
    {
        config.LoginPath = "/login";
        config.LogoutPath = "/logout";
        config.AccessDeniedPath = "/login";
    });
});

// Add authorization services
builder.Services.AddAuthorizationCore();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDepartmentCommandHandler).Assembly));

var app = builder.Build();

// Seed test user in development only, not during testing
if (app.Environment.IsDevelopment() && !app.Environment.EnvironmentName.Contains("Test"))
{
    try
    {
        using var scope = app.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        await SeedTestUserAsync(userManager);
    }
    catch
    {
        // Ignore seeding errors in test environments
    }
}

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

static async Task SeedTestUserAsync(UserManager<ApplicationUser> userManager)
{
    var testUser = await userManager.FindByNameAsync("test@example.com");
    if (testUser == null)
    {
        testUser = new ApplicationUser
        {
            UserName = "test@example.com",
            Email = "test@example.com",
            EmailConfirmed = true,
            FirstName = "Test",
            LastName = "User",
            ApplicationUserId = EastSeat.ResourceIdea.Domain.Users.ValueObjects.ApplicationUserId.Create(Guid.NewGuid()),
            TenantId = EastSeat.ResourceIdea.Domain.Tenants.ValueObjects.TenantId.Create(Guid.NewGuid())
        };

        var result = await userManager.CreateAsync(testUser, "Test123!");
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to create test user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}

// Make Program accessible for testing
public partial class Program { }

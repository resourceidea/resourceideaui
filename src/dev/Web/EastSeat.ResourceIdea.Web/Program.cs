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
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

// Add authorization services
builder.Services.AddAuthorizationCore();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDepartmentCommandHandler).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseStaticFiles();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();

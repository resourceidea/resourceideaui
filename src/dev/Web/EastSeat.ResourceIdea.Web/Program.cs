using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.DataStore;
using EastSeat.ResourceIdea.DataStore.Configuration.DatabaseStartup;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Web;
using EastSeat.ResourceIdea.Web.Auth.Services;
using EastSeat.ResourceIdea.Web.Components;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using MediatR;
using EastSeat.ResourceIdea.Application.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpContextAccessor();

var sqlServerConnectionString = StartupConfiguration.GetSqlServerConnectionString();
builder.Services.AddDbContext<ResourceIdeaDBContext>(options => options.UseSqlServer(
    sqlServerConnectionString));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
.AddEntityFrameworkStores<ResourceIdeaDBContext>()
.AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);

builder.Services.AddTransient<IUserAuthenticationService, UserAuthenticationService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
}).AddIdentityCookies();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
});

builder.Services.AddDataStoreServices(sqlServerConnectionString);

// Add logging for diagnostics
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
builder.Services.Configure<DatabaseStartupTasksConfig>(builder.Configuration.GetSection("DatabaseStartupTasks"));

builder.Services.AddAutoMapper(typeof(ResourceIdeaMappingProfile));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies([
    typeof(IUserAuthenticationService).Assembly,
    typeof(CreateClientCommand).Assembly]));

var app = builder.Build();

await app.RunDatabaseStartupTasks();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

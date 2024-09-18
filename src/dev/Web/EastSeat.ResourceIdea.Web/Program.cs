using EastSeat.ResourceIdea.Application.Features.ApplicationUsers.Handlers;
using EastSeat.ResourceIdea.DataStore;
using EastSeat.ResourceIdea.DataStore.Configuration.DatabaseStartup;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Web;
using EastSeat.ResourceIdea.Web.Components;

using MediatR;

using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddServerSideBlazor();

// Add datastore service.
var sqlServerConnectionString = StartupConfiguration.GetSqlServerConnectionString();
builder.Services.AddResourceIdeaDbContext(sqlServerConnectionString);

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();

builder.Services.AddMediatR(typeof(LoginCommandHandler).Assembly);

builder.Services.Configure<DatabaseStartupTasksConfig>(builder.Configuration.GetSection("DatabaseStartupTasks"));

var app = builder.Build();

app.RunDatabaseStartupTasks();

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

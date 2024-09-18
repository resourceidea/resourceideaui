using EastSeat.ResourceIdea.Web.Components;
using EastSeat.ResourceIdea.DataStore;
using EastSeat.ResourceIdea.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Application.Features.ApplicationUsers.Handlers;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddServerSideBlazor();

// Add datastore service.
var sqlServerConnectionString = StartupConfiguration.GetSqlServerConnectionString();
builder.Services.AddResourceIdeaDataStore(sqlServerConnectionString);

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();

builder.Services.AddMediatR(typeof(LoginCommandHandler).Assembly);

var app = builder.Build();

// Automatically apply migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ResourceIdeaDBContext>();
    dbContext.Database.Migrate();
}

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

app.ApplyMigrations();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

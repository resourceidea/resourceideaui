using EastSeat.ResourceIdea.Application;
using EastSeat.ResourceIdea.Persistence;

using Microsoft.EntityFrameworkCore;

namespace EastSeat.ResourceIdea.Api;

public static class StartupExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationServices();
        builder.Services.AddPersistentServices(builder.Configuration);
        builder.Services.AddAuthorization();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        });
    }

    public static void ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            // Dev only configuration
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseCors("Open");

        app.UseAuthorization();
    }

    public async static Task MigrateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        try
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<ResourceIdeaDbContext>();
            if (dbContext is not null)
            {
                await dbContext.Database.MigrateAsync();
            }
        }
        catch (Exception)
        {
            // TODO: Add logging here.
            throw;
        }
    }
}

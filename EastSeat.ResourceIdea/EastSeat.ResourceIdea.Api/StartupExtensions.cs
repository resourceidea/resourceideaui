using EastSeat.ResourceIdea.Application;
using EastSeat.ResourceIdea.Persistence;

using Microsoft.EntityFrameworkCore;

namespace EastSeat.ResourceIdea.Api;

public static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationServices();
        builder.Services.AddPersistentServices(builder.Configuration);

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            // Dev only configuration
        }

        app.UseHttpsRedirection();

        return app;
    }

    public async static Task MigrateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        try
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ResourceIdeaDbContext>();
            if (context is not null)
            {
                await context.Database.MigrateAsync();
            }
        }
        catch (Exception)
        {
            // TODO: Add logging here.
            throw;
        }
    }
}

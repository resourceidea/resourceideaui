using EastSeat.ResourceIdea.Api;
using EastSeat.ResourceIdea.Api.AppRoutes;

var builder = WebApplication.CreateBuilder(args);

var app = builder
    .ConfigureServices()
    .ConfigurePipeline()
    .MapRoutes();

await app.MigrateDatabaseAsync();

app.Run();

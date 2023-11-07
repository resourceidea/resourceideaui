using EastSeat.ResourceIdea.Api;
using EastSeat.ResourceIdea.Api.AppRoutes;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

app.ConfigurePipeline();
app.MapEndpoints();

await app.MigrateDatabaseAsync();

app.Run();
